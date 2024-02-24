using Domain.Base.Messages;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Domain.Base.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task<TResponse> EnviarComando<TCommand, TResponse>(TCommand command) where TCommand : Command<TResponse>;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }
}
