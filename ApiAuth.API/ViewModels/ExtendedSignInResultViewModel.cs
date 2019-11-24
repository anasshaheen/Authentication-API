using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.ViewModels
{
    public class ExtendedSignInResultViewModel
    {
        public ExtendedSignInResultViewModel(SignInResult result, IList<string> roles = null)
        {
            Result = result;
            Roles = roles;
        }

        public SignInResult Result { get; }
        public IList<string> Roles { get; }
    }
}
