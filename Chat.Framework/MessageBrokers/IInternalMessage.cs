namespace Chat.Framework.MessageBrokers;

public interface IInternalMessage
{
    string? Token { get; set; }
}