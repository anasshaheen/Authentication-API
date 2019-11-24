using ApiAuth.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Abstraction.IServices
{
    public interface IEmailTemplateGenerator
    {
        Task<string> RenderActionTemplate(string title, List<string> body, UrlEmailTemplateViewModel url = null);
    }
}