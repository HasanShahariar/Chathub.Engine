using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Application.Features.Auth.Models
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }           // JWT token
        public string RefreshToken { get; set; }     // Optional, if you are using refresh tokens
        public string ErrorMessage { get; set; }     // Error message if authentication failed
        public DateTime Expiration { get; set; }     // Token expiration time

        // Additional user info if needed
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
