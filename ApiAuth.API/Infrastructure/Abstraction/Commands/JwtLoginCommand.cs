using ApiAuth.API.DtoS;
using ApiAuth.API.ViewModels;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class JwtLoginCommand : IRequest<LoginResultViewModel>
    {
        public JwtLoginCommand(LoginDto input, bool byPassPassword = false)
        {
            Input = input;
            ByPassPassword = byPassPassword;
        }

        public LoginDto Input { get; }
        public bool ByPassPassword { get; }
    }
}