using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class RegisterUserRequest : IRequest<IdentityResult>
    {
        public RegisterUserRequest(RegisterDto input, string requestUrlSchema)
        {
            Input = input;
            RequestUrlSchema = requestUrlSchema;
        }

        public RegisterDto Input { get; }
        public string RequestUrlSchema { get; }
    }
}
