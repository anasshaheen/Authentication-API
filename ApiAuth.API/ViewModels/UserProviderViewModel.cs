using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ApiAuth.API.ViewModels
{
    public class UserProviderViewModel
    {
        public List<AuthenticationScheme> UnusedLoginProviders { get; set; }
        public List<UserLoginInfo> UsedLoginProvider { get; set; }
    }
}
