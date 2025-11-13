using System;

namespace LoginMvcApp.Models;

public class RegisterViewModel
    {
        // input fields
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;

        // For the SOAP call
        public int CountryID { get; set; } = 1;  // pick 1 as default
        public int AID { get; set; } = 0;        // affiliate id / app id – default 0

        // result
        public bool? Success { get; set; }
        public string? Message { get; set; }

        // raw service JSON/string
        public string? RawResponse { get; set; }

        // parsed JSON result (reuse same structure as login)
        public LoginApiPayload? Details { get; set; }
    }
