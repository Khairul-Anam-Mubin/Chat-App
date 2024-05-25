using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Infrastructure.Shared.Controllers;

public abstract class ACommonController : ControllerBase
{
    protected readonly ICommandExecutor CommandExecutor;
    protected readonly IQueryExecutor QueryExecutor;

    protected ACommonController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        CommandExecutor = commandExecutor;
        QueryExecutor = queryExecutor;
    }

    protected async Task<IResult> GetCommandResponseAsync<TCommand>(TCommand command) 
        where TCommand : class, ICommand
    {
        return await CommandExecutor.ExecuteAsync(command);
    }

    protected async Task<IResult<TResponse>> GetCommandResponseAsync<TCommand, TResponse>(TCommand command) 
        where TCommand : class, ICommand<TResponse>
        where TResponse : class
    {
        return await CommandExecutor.ExecuteAsync<TCommand, TResponse>(command);
    }

    protected async Task<IResult<TResponse>> GetQueryResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IQuery<TResponse>
        where TResponse : class
    {
        return await QueryExecutor.ExecuteAsync<TQuery, TResponse>(query);
    }
}