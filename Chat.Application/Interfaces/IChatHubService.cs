namespace Chat.Application.Interfaces;

public interface IChatHubService
{
    Task SendAsync<T>(string userId, T message, string method = "ReceivedChat");
}