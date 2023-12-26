using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Notification.Application.Consumers;

public class SendEmailCommandConsumer : ACommandConsumer<SendEmailCommand>
{
    private readonly IEmailSender _emailSender;

    public SendEmailCommandConsumer(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
        
    protected override async Task<IResult> OnConsumeAsync(
        SendEmailCommand command, IMessageContext<SendEmailCommand>? context = null)
    {
        if (command.Email is null)
        {
            return Result.Error("Email model error");
        }

        await _emailSender.SendAsync(command.Email);

        return Result.Success("Email Sent");
    }
}