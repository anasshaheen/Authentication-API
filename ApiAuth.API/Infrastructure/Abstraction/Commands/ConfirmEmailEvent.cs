using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class ConfirmEmailEvent : INotification
    {
        public ConfirmEmailEvent(string code, string userId)
        {
            Code = code;
            UserId = userId;
        }

        public string Code { get; }
        public string UserId { get; }
    }
}