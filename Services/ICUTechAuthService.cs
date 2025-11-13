using System;
using System.Threading.Tasks;
using LoginMvcApp.Models;
using ServiceReference;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace LoginMvcApp.Services;

public class ICUTechAuthService : IICUTechAuthService
{

    private readonly ILogger<ICUTechAuthService> _logger;
    public ICUTechAuthService(ILogger<ICUTechAuthService> logger)
    {
        _logger = logger;
    }

    public async Task<LoginViewModel> LoginAsync(string username, string password, string ip)
    {
        var model = new LoginViewModel { Username = username };

        try
        {
            var client = new ICUTechClient(ICUTechClient.EndpointConfiguration.IICUTechPort);

            // Call the SOAP Login operation
            var resp = await client.LoginAsync(username, password, ip);

            model.RawResponse = resp.@return;
            _logger.LogInformation("Login response: {Response}", resp.@return);

            // You need to decide what "success" means based on actual returned string.
            // For the test, you can treat non-empty as success.
            LoginApiPayload? payload = null;

            try
            {
                payload = JsonSerializer.Deserialize<LoginApiPayload>(resp.@return);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse JSON login response: {Response}", resp.@return);
            }

            if (payload != null)
            {
                model.Details = payload;
                model.Success = payload.ResultCode == 0;
                model.Message = payload.ResultMessage;
            }
            else
            {
                model.Success = !string.IsNullOrEmpty(resp.@return);
                model.Message = resp.@return;
            }
        }
        catch (Exception ex)
        {
            model.Success = false;
            model.Message = "Error contacting login service.";
            model.RawResponse = ex.Message;
        }

        return model;
    }

    public async Task<RegisterViewModel> RegisterAsync(RegisterViewModel model, string ip)
    {
        _logger.LogInformation("RegisterNewCustomer attempt: Email={Email}, IP={IP}", model.Email, ip);

        try
        {
            var client = new ICUTechClient(ICUTechClient.EndpointConfiguration.IICUTechPort);

            // WSDL call:
            // RegisterNewCustomerAsync(string Email, string Password, string FirstName,
            //                          string LastName, string Mobile, int CountryID, int aID, string SignupIP)
            var resp = await client.RegisterNewCustomerAsync(
                model.Email,
                model.Password,
                model.FirstName,
                model.LastName,
                model.Mobile,
                model.CountryID,
                model.AID,
                ip
            );

            model.RawResponse = resp.@return;
            _logger.LogInformation("Register response: {Response}", resp.@return);

            LoginApiPayload? payload = null;
            try
            {
                payload = JsonSerializer.Deserialize<LoginApiPayload>(resp.@return);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Failed to parse JSON register response: {Response}",
                    resp.@return);
            }

            if (payload != null)
            {
                model.Details = payload;
                model.Success = payload.ResultCode == 0;
                model.Message = payload.ResultMessage;
            }
            else
            {
                model.Success = !string.IsNullOrEmpty(resp.@return);
                model.Message = resp.@return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling ICUTech RegisterNewCustomer");

            model.Success = false;
            model.Message = "Error contacting registration service.";
            model.RawResponse = ex.Message;
        }

        return model;
    }
    
}
