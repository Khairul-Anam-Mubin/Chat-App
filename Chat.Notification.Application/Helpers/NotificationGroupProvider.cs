namespace Chat.Notification.Application.Helpers;

public class NotificationGroupProvider
{
    public static string GetGroupByUserId(string userId)
    {
        return $"Group-{userId}";
    }
}