using Chat.Framework.CQRS;

namespace Chat.Framework.Proxy;
public interface ICommandQueryProxy : IProxy
{
    Task<CommandResponse> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task<QueryResponse> GetQueryResponseAsync<TQuery>(TQuery query) where TQuery : IQuery;
}