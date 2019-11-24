using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class SendVerificationEmailEvent : INotification
    {
        public SendVerificationEmailEvent(string userName, string requestSchema)
        {
            UserName = userName;
            RequestSchema = requestSchema;
        }

        public string UserName { get; }
        public string RequestSchema { get; }
    }
}