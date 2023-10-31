using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public abstract class ACommandHandler<TCommand, TResponse> : 
    IRequestHandler<TCommand, TResponse>
    where TCommand : class, ICommand
    where TResponse : class, IResponse
{
    protected abstract Task<TResponse> OnHandleAsync(TCommand command);

    public async Task<TResponse> HandleAsync(TCommand command)
    {
        Console.WriteLine($"OnHandleAsync of : {GetType().Name}\n");

        try
        {
            var response = await OnHandleAsync(command);

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            var response = command.CreateResponse();
            response.SetErrorMessage(e.Message);
            
            return response as TResponse;
        }
    }
}