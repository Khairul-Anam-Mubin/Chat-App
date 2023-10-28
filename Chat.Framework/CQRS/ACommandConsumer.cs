using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class ACommandConsumer<TCommand> : 
    AMessageConsumer<TCommand>,
    IRequestHandler<TCommand, CommandResponse>
    where TCommand : class, ICommand
{
    protected abstract Task<CommandResponse> OnConsumeAsync(TCommand command, IMessageContext<TCommand>? context = null);

    public override async Task Consume(IMessageContext<TCommand> context)
    {
        await OnConsumeAsync(context.Message, context);
    }

    public async Task<CommandResponse> HandleAsync(TCommand request)
    {
        try
        {
            return await OnConsumeAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            var response = request.CreateResponse();
            
            response.SetErrorMessage(e.Message);
            
            return response;
        }
    }
}