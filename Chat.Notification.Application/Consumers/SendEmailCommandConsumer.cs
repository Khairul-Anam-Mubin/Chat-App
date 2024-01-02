using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;
using Microsoft.FeatureManagement;

namespace Chat.Notification.Application.Consumers;

public class SendEmailCommandConsumer : ACommandConsumer<SendEmailCommand>
{
    private readonly IEmailSender _emailSender;
    private readonly IFeatureManager _featureManager;

    public SendEmailCommandConsumer(IEmailSender emailSender, IFeatureManager featureManager)
    {
        _emailSender = emailSender;
        _featureManager = featureManager;
    }
        
    protected override async Task<IResult> OnConsumeAsync(
        SendEmailCommand command, IMessageContext<SendEmailCommand>? context = null)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.Email))
        {
            return Result.Ignored("Feature Disabled");
        }

        if (command.Email is null)
        {
            return Result.Error("Email model error");
        }

        await _emailSender.SendAsync(command.Email);

        return Result.Success("Email Sent");
    }
}