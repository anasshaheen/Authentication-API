using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class SetPasswordRequest : IRequest<IdentityResult>
    {
        public SetPasswordRequest(string userName, SetPasswordDto input)
        {
            UserName = userName;
            Input = input;
        }

        public string UserName { get; }
        public SetPasswordDto Input { get; }
    }
}