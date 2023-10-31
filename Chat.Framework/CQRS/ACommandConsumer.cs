﻿using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class ACommandConsumer<TCommand, TResponse> : 
    AMessageConsumer<TCommand>,
    IRequestHandler<TCommand, TResponse>
    where TCommand : class, ICommand
    where TResponse : class, IResponse
{
    protected abstract Task<TResponse> OnConsumeAsync(TCommand command, IMessageContext<TCommand>? context = null);

    public override async Task Consume(IMessageContext<TCommand> context)
    {
        await OnConsumeAsync(context.Message, context);
    }

    public async Task<TResponse> HandleAsync(TCommand request)
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
            
            return response as TResponse;
        }
    }
}