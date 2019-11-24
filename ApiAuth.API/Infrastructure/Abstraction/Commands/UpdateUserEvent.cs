using ApiAuth.API.DtoS;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class UpdateUserEvent : INotification
    {
        public UpdateUserEvent(string userName, UpdateUserDto input)
        {
            UserName = userName;
            Input = input;
        }

        public string UserName { get; }
        public UpdateUserDto Input { get; }
    }
}
