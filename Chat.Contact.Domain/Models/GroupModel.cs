namespace Chat.Contact.Domain.Models;

public class GroupModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public GroupModel(string name, string createdBy)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }
}