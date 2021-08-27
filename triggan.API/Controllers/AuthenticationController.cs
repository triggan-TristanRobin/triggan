using triggan.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using triggan.BlogManager;
using triggan.BlogModel;

namespace ExpensesTracker.Controllers
{
    [ApiController]
    [Authorize]
    [Route("")]
    public class AuthenticationController : ControllerBase
    {
        public IServiceProvider ServiceProvider { get; set; }
        private TrigganContext trigganContext => ServiceProvider.GetService<TrigganContext>();
        private AppSettings appSettings;

        public AuthenticationController(IServiceProvider serviceProvider, IOptions<AppSettings> appSettings = null)
        {
            ServiceProvider = serviceProvider;
            this.appSettings = appSettings?.Value;
        }

        [AllowAnonymous]
        [HttpPost("Signup")]
        public IActionResult Register([FromBody] User newUserCreds)
        {
            using var context = trigganContext;

            if (!this.User.HasClaim(claim => claim.Type == ClaimTypes.Name))
            {
                newUserCreds.Role = UserRole.Basic;
                newUserCreds.Active = false;
            }
            else if (newUserCreds.Role == UserRole.Admin)
            {
                newUserCreds.Active = true;
            }

            var hash = new PasswordHasher<User>().HashPassword(newUserCreds, newUserCreds.Password);

            var addedUser = context.Add(newUserCreds.WithHashedPassword(hash));
            var changedEntities = context.SaveChanges();
            return changedEntities == 1 ? Ok(addedUser.Entity.WithoutPassword()) : StatusCode(500);
        }

        [AllowAnonymous]
        [HttpPost("Signin")]
        public IActionResult Signin([FromBody] UserSigninInfos signinInfos)
        {
            using var context = trigganContext;
            var user = context.Users.SingleOrDefault(u => u.Username == signinInfos.Username);

            var hashVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user?.Password ?? "", signinInfos.Password);

            if (user == null)
            {
                return Unauthorized(SigninErrorType.UserNotFound);
            }
            else if (hashVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(SigninErrorType.PasswordError);
            }
            else if(!user.Active)
            {
                return Unauthorized(SigninErrorType.AccountNotActivated);
            }
            else
            {
                user.Token = GetToken(user);
                return Ok(user.WithoutPassword());
            }
        }

        private string GetToken(User user)
        {
            if(!user.Active)
            {
                throw new ArgumentException("Cannot retrieve token for a non activated account");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}