using System;

namespace LoginMvcApp.Models;

public class LoginViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool? Success { get; set; }
    public string? Message { get; set; }
    public string? RawResponse { get; set; }

    public LoginApiPayload? Details { get; set; }
}
