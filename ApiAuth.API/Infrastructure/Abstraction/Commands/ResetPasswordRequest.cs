using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class ResetPasswordRequest : IRequest<IdentityResult>
    {
        public ResetPasswordRequest(ResetPasswordDto input)
        {
            Input = input;
        }

        public ResetPasswordDto Input { get; }
    }
}