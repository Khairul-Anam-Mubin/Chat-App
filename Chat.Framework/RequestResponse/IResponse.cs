using Chat.Framework.Enums;
using Chat.Framework.Interfaces;

namespace Chat.Framework.RequestResponse;

public interface IResponse : IMetaDataDictionary
{
    string Message { get; set; }
    ResponseStatus? Status { get; set; }
    IResponse ErrorMessage(string message);
    IResponse SuccessMessage(string message);
}

public interface IResponse<TResponse> : IResponse
{
    TResponse? Response { get; set; }
}