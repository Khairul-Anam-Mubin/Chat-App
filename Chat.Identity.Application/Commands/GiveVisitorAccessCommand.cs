using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands
{
    public class GiveVisitorAccessCommand : ICommand
    {
        public string UserId { get; set; }
    }
}
