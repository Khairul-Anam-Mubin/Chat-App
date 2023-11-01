using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public interface ICommandHandler<in TCommand, TResponse> : 
    IRequestHandler<TCommand, TResponse>
    where TCommand : class, ICommand
    where TResponse : class, IResponse {}