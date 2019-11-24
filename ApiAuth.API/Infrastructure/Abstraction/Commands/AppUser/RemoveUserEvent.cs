using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser
{
    public class RemoveUserEvent : INotification
    {
        public RemoveUserEvent(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}
