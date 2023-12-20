using Chat.Framework.Enums;
using Chat.Framework.Mediators;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public class CommandExecutor : ICommandExecutor
{
    private readonly IMediator _mediator;

    public CommandExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IResult> ExecuteAsync<TCommand>(TCommand command) 
        where TCommand : class, ICommand
    {
        try
        {
            var result = await _mediator.SendAsync<TCommand, IResult>(command);
            result.Status ??= ResponseStatus.Success;
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return Result.Error(e.Message);
        }
    }

    public async Task<IResult<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command) 
        where TCommand : class, ICommand
        where TResponse : class
    {
        try
        {
            var result = await _mediator.SendAsync<TCommand, IResult<TResponse>>(command);
            result.Status ??= ResponseStatus.Success;
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return Result.Error<TResponse>(e.Message);
        }
    }
}