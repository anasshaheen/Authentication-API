using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class SetPasswordCommand : IRequest<IdentityResult>
    {
        public SetPasswordCommand(string userName, SetPasswordDto input)
        {
            UserName = userName;
            Input = input;
        }

        public string UserName { get; }
        public SetPasswordDto Input { get; }
    }
}