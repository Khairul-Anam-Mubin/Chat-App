using Chat.Framework.Enums;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;

namespace Chat.Framework.CQRS;

public class CommandExecutor : ICommandExecutor
{
    private readonly IMediator _mediator;

    public CommandExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command)
        where TCommand : class
        where TResponse : class, IResponse
    {
        try
        {
            var response = await _mediator.SendAsync<TCommand, TResponse>(command);

            response.Status ??= ResponseStatus.Success;

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            var response = Response.Error(e);

            return (response as TResponse)!;
        }
    }

    public async Task<IResponse> ExecuteAsync<TCommand>(TCommand command) 
        where TCommand : class
    {
        return await ExecuteAsync<TCommand, IResponse>(command);
    }
}