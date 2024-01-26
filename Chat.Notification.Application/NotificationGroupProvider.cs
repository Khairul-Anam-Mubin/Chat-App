namespace Chat.Notification.Application;

public class NotificationGroupProvider
{
    public static string GetGroupByUserId(string userId)
    {
        return $"Group-{userId}";
    }
}