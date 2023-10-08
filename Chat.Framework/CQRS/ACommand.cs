using Chat.Framework.Enums;
using Chat.Framework.Models;

namespace Chat.Framework.CQRS;

public abstract class ACommand : MetaDataDictionary, ICommand
{
    public string ApiUrl { get; set; }
    public bool FireAndForget { get; set; }

    public abstract void ValidateCommand();
    protected ACommand()
    {
        FireAndForget = false;
        ApiUrl = string.Empty;
    }

    public CommandResponse CreateResponse()
    {
        return new CommandResponse
        {
            Name = GetType().Name,
            Status = ResponseStatus.Success
        };
    }

    public CommandResponse CreateResponse(CommandResponse response)
    {
        response.Name = GetType().Name;
        return response;
    }
}