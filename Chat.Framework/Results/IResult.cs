using Chat.Framework.Interfaces;

namespace Chat.Framework.Results;

public interface IResult : IMetaDataDictionary
{
    string Message { get; }
    ResponseStatus Status { get; }

    bool IsSuccess { get; }

    bool IsFailure { get; }
}

public interface IResult<TResponse> : IResult
{
    TResponse? Value { get; }
}