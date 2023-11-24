using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using Chat.Framework.Extensions;

namespace Chat.Framework.EmailSenders;

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailConfig _emailConfig;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _emailConfig = configuration.TryGetConfig<EmailConfig>();
    }

    public async Task SendAsync(Email email)
    {
        var mailMessage = CreateMailMessage(email);

        try
        {
            await SendAsync(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private MailMessage CreateMailMessage(Email email)
    {
        var mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(_emailConfig.From);

        mailMessage.Subject = email.Subject;

        foreach (var to in email.To)
        {
            mailMessage.To.Add(new MailAddress(to));
        }

        mailMessage.Body = email.Content;
        mailMessage.IsBodyHtml = email.IsHtmlContent;

        foreach (var attachment in email.FilePaths)
        {
            mailMessage.Attachments.Add(new Attachment(attachment));
        }

        return mailMessage;
    }

    private async Task SendAsync(MailMessage message)
    {
        using var client = new SmtpClient(_emailConfig.Server)
        {
            Port = _emailConfig.Port,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password),
            EnableSsl = true
        };

        try
        {
            client.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        await Task.CompletedTask;
    }
}