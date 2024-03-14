using Chat.Framework.Interfaces;

namespace Chat.Framework.Results;

public interface IResult : IMetaDataDictionary
{
    string Message { get; }
    ResponseStatus Status { get; }

    bool IsSuccess { get; }

    bool IsFailure { get; }

    IResult SetMessage(string message);
}

public interface IResult<out TResponse> : IResult
{
    TResponse? Value { get; }
}