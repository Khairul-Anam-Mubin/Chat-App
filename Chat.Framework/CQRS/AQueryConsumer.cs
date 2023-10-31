using Chat.Framework.Enums;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class AQueryConsumer<TQuery, TResponse> :
    AMessageConsumer<TQuery>,
    IRequestHandler<TQuery, TResponse>
    where TResponse : class, IResponse
    where TQuery : class, IQuery
{
    protected abstract Task<TResponse> OnConsumeAsync(
        TQuery query, IMessageContext<TQuery>? context = null);

    public override async Task Consume(IMessageContext<TQuery> context)
    {
        TResponse response;

        try
        {
            response = await OnConsumeAsync(context.Message, context);
            response.Status = ResponseStatus.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            response = context.Message.CreateResponse() as TResponse;
            response.Status = ResponseStatus.Error;
            response.Message = e.Message;
        }

        try
        {
            await context.RespondAsync(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<TResponse> HandleAsync(TQuery request)
    {
        try
        {
            var response = await OnConsumeAsync(request);
            response.Status = ResponseStatus.Success;

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            var response = request.CreateResponse() as TResponse;

            response.Status = ResponseStatus.Error;
            response.Message = e.Message;
            
            return response;
        }
    }
}