using LoginMvcApp.Models;

namespace LoginMvcApp.Services
{
    public interface IICUTechAuthService
    {
        Task<LoginViewModel> LoginAsync(string username, string password, string ip);

        Task<RegisterViewModel> RegisterAsync(RegisterViewModel model, string ip);
    }
}