using ApiAuth.API.DtoS;
using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class ForgotPasswordEvent : INotification
    {
        public ForgotPasswordEvent(string userName, ForgotPasswordDto input, string requestSchema)
        {
            UserName = userName;
            Input = input;
            RequestSchema = requestSchema;
        }

        public string UserName { get; }
        public ForgotPasswordDto Input { get; }
        public string RequestSchema { get; }
    }
}