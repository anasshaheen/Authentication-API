using MediatR;

namespace ApiAuth.API.Infrastructure.Abstraction.Commands
{
    public class DeleteProviderEvent : INotification
    {
        public DeleteProviderEvent(string userName, string providerKey, string loginProvider)
        {
            UserName = userName;
            ProviderKey = providerKey;
            LoginProvider = loginProvider;
        }

        public string UserName { get; }
        public string ProviderKey { get; }
        public string LoginProvider { get; }
    }
}