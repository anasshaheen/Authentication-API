using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public RegisterUserCommand(RegisterDto input, string requestUrlSchema)
        {
            Input = input;
            RequestUrlSchema = requestUrlSchema;
        }

        public RegisterDto Input { get; }
        public string RequestUrlSchema { get; }
    }
}
