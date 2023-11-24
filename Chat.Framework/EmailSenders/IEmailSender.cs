namespace Chat.Framework.EmailSenders;

public interface IEmailSender
{
    Task SendAsync(Email email);
}