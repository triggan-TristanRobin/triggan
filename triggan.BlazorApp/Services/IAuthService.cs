using System.Threading.Tasks;
using triggan.BlogModel;

namespace triggan.BlazorApp.Services
{
    public interface IAuthService
    {
        Task<User> Register(User registerModel);
        Task<User> Login(UserSigninInfos signinInfos);
        Task Logout();
    }
}