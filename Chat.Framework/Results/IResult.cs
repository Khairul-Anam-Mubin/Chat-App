using Chat.Framework.Interfaces;

namespace Chat.Framework.Results;

public interface IResult : IMetaDataDictionary
{
    string Message { get; set; }
    ResponseStatus? Status { get; set; }

    bool IsSuccess();
}

public interface IResult<TResponse> : IResult
{
    TResponse? Value { get; set; }
}