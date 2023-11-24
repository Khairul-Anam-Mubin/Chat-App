namespace Chat.Framework.EmailSenders;

public class Email
{
    public List<string> To { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsHtmlContent { get; set; }
    public List<string> FilePaths { get; set; }

    public Email()
    {
        To = new List<string>();
        FilePaths = new List<string>();
    }
}