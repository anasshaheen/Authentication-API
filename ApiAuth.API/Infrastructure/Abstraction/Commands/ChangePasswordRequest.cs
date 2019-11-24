using ApiAuth.API.DtoS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class ChangePasswordRequest : IRequest<IdentityResult>
    {
        public ChangePasswordRequest(string userName, ChangePasswordDto input)
        {
            UserName = userName;
            Input = input;
        }

        public string UserName { get; }
        public ChangePasswordDto Input { get; }
    }
}