using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiAuth.API.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary AddErrors(this ModelStateDictionary modelState, IdentityResult identityResult)
        {
            foreach (var e in identityResult.Errors)
            {
                modelState.TryAddModelError(e.Code, e.Description);
            }

            return modelState;
        }
    }
}
