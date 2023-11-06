using Chat.Framework.Enums;
using Chat.Framework.Interfaces;

namespace Chat.Framework.Models;

public class Response : MetaDataDictionary, IResponse
{
    public string Message { get; set; } = string.Empty;
    public ResponseStatus Status { get; set; }

    public void SetErrorMessage(string message)
    {
        Message = message;
        Status = ResponseStatus.Error;
    }

    public void SetSuccessMessage(string message)
    {
        Message = message;
        Status = ResponseStatus.Success;
    }

    public static Response Create()
    {
        return new Response
        {
            Status = ResponseStatus.Success
        };
    }
}

public class ResponseEnvelope<TResponse> : Response, IResponse<TResponse>
{
    public TResponse? Response { get; set; }
}