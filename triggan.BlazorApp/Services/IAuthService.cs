using System;
using System.Threading.Tasks;
using triggan.BlogModel;

namespace triggan.BlazorApp.Services
{
    public interface IAuthService
    {
        Task<User> Register(User registerModel);
        Task<User> Signin(UserSigninInfos signinInfos);
        Task Signout();
    }
}