using Chat.Framework.Enums;
using Chat.Framework.Interfaces;

namespace Chat.Framework.Models;

public class Response : MetaDataDictionary, IResponse
{
    public string Message { get; set; } = string.Empty;
    public ResponseStatus? Status { get; set; }

    public IResponse ErrorMessage(string message)
    {
        Message = message;
        Status = ResponseStatus.Error;
        return this;
    }

    public IResponse SuccessMessage(string message)
    {
        Message = message;
        Status = ResponseStatus.Success;
        return this;
    }

    public static IResponse Success()
    {
        return new Response
        {
            Status = ResponseStatus.Success
        };
    }

    public static IResponse Error()
    {
        return new Response
        {
            Status = ResponseStatus.Error
        };
    }

    public static IResponse Error(Exception ex)
    {
        return Error().ErrorMessage(ex.Message);
    }

    public static IResponse Success(string message)
    {
        return Success().SuccessMessage(message);
    }

    public static IResponse Error(string message)
    {
        return Error().ErrorMessage(message);
    }
}

public class ResponseEnvelope<TResponse> : Response, IResponse<TResponse>
{
    public TResponse? Response { get; set; }
}