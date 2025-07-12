using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.IdentityProvider.Application.Login
{
    public sealed record LoginResponse
    {
        public LoginResponse(LoginUserResponse user, string token)
        {
            User = user;
            Token = token;
        }
        public LoginUserResponse User { get; set; }
        public string Token { get; set; }
    }

    public sealed record LoginUserResponse
    {
        public LoginUserResponse(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
    }

}
