using Chat.Domain.Models;

namespace Chat.Application.Interfaces;

public interface IChatRepository
{
    Task<bool> SaveChatModelsAsync(List<ChatModel> chatModels);
    Task<bool> SaveChatModelAsync(ChatModel chatModel);
    Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit);
    Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId);
}