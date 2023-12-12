using Chat.Framework.Enums;
using Chat.Framework.Interfaces;

namespace Chat.Framework.Results;

public interface IResult : IMetaDataDictionary
{
    public string Message { get; set; }
    public ResponseStatus? Status { get; set; }
}

public interface IResult<TResponse> : IResult
{
    TResponse? Response { get; set; }
}