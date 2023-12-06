﻿using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;
using Chat.Framework.MessageBrokers;
using Chat.Framework.RequestResponse;

namespace Chat.Contact.Application.Consumers;

public class SendEmailCommandConsumer : ACommandConsumer<SendEmailCommand, IResponse>
{
    private readonly IEmailSender _emailSender;

    public SendEmailCommandConsumer(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
        
    protected override async Task<IResponse> OnConsumeAsync(
        SendEmailCommand command, IMessageContext<SendEmailCommand>? context = null)
    {
        if (command.Email is null)
        {
            return Response.Error("Email model error");
        }

        await _emailSender.SendAsync(command.Email);

        return Response.Success("Email Sent");
    }
}