using ApiAuth.API.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Abstraction.IServices
{
    public interface IEmailDispatcher : IEmailSender
    {
        Task SaveEmail(Email email);
    }
}
