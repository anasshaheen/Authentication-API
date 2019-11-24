using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class ResetPasswordCommand : IRequest<IdentityResult>
    {
        public ResetPasswordCommand(ResetPasswordDto input)
        {
            Input = input;
        }

        public ResetPasswordDto Input { get; }
    }
}