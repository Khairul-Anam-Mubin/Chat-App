using Chat.Framework.Enums;

namespace Chat.Framework.Interfaces;

public interface IResponse : IMetaDataDictionary
{
    string Message { get; set; }
    ResponseStatus Status { get; set; }
    void SetErrorMessage(string message);
    void SetSuccessMessage(string message);
}

public interface IResponse<TResponse> : IResponse
{
    TResponse? Response { get; set; }
}