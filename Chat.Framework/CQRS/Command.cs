using Chat.Framework.Mediators;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public interface ICommand {}

public interface ICommandHandler<in TCommand> : IHandler<TCommand, IResult> 
    where TCommand: class, ICommand {}

public interface ICommandHandler<in TCommand, TResponse> : IHandler<TCommand, IResult<TResponse>> 
    where TCommand : class, ICommand
    where TResponse : class {}