using ApiAuth.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Abstraction.IServices
{
    public interface IJwtFactory
    {
        Task<AccessTokenViewModel> GenerateEncodedToken(string userName, IList<string> roles);
    }
}
