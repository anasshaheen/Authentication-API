using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class ChangePasswordCommand : IRequest<IdentityResult>
    {
        public ChangePasswordCommand(string userName, ChangePasswordDto input)
        {
            UserName = userName;
            Input = input;
        }

        public string UserName { get; }
        public ChangePasswordDto Input { get; }
    }
}