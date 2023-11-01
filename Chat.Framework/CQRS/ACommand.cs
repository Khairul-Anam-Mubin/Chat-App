using Chat.Framework.Enums;
using Chat.Framework.Interfaces;
using Chat.Framework.Models;

namespace Chat.Framework.CQRS;

public abstract class ACommand : MetaDataDictionary, ICommand
{
    public string ApiUrl { get; set; }
    public bool FireAndForget { get; set; }

    protected ACommand()
    {
        FireAndForget = false;
        ApiUrl = string.Empty;
    }

    public IResponse CreateResponse()
    {
        return new Response
        {
            Status = ResponseStatus.Success
        };
    }
}