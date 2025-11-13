using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginMvcApp.Models;
using LoginMvcApp.Services;

namespace LoginMvcApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IICUTechAuthService _soapAuthClient;

        public AuthController(IICUTechAuthService soapAuthClient)
        {
            _soapAuthClient = soapAuthClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                model.Success = false;
                model.Message = "Username and password are required.";
                return View(model);
            }

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

            var result = await _soapAuthClient.LoginAsync(model.Username, model.Password, ip);

            // result already has Success, Message, RawResponse set
            return View(result);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                model.Success = false;
                model.Message = "Email and password are required.";
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                model.Success = false;
                model.Message = "Passwords do not match.";
                return View(model);
            }

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

            var result = await _soapAuthClient.RegisterAsync(model, ip);

            return View(result);
        }
    }
}
