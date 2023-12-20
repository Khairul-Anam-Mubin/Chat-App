using Chat.Framework.Enums;
using Chat.Framework.Models;

namespace Chat.Framework.Results;

public class Result : MetaDataDictionary, IResult
{
    public string Message { get; set; } = string.Empty;
    public ResponseStatus? Status { get; set; }

    public static IResult Success(string message = "")
    {
        return new Result
        {
            Status = ResponseStatus.Success,
            Message = message
        };
    }

    public static IResult Error(string message = "")
    {
        return new Result
        {
            Status = ResponseStatus.Error,
            Message = message
        };
    }

    public static IResult<TResponse> Success<TResponse>(TResponse? response)
    {
        return new Result<TResponse>
        {
            Status = ResponseStatus.Success,
            Response = response
        };
    }

    public static IResult<TResponse> Error<TResponse>(TResponse? response)
    {
        return new Result<TResponse>
        {
            Status = ResponseStatus.Error,
            Response = response,
        };
    }

    public static IResult<TResponse> Success<TResponse>(TResponse? response, string message)
    {
        return new Result<TResponse>
        {
            Status = ResponseStatus.Success,
            Response = response,
            Message = message
        };
    }

    public static IResult<TResponse> Error<TResponse>(TResponse? response, string message)
    {
        return new Result<TResponse>
        {
            Status = ResponseStatus.Error,
            Response = response,
            Message = message
        };
    }

    public static IResult<TResponse> Success<TResponse>(string message = "")
    {
        return new Result<TResponse>
        {
            Status = ResponseStatus.Success,
            Message = message
        };
    }

    public static IResult<TResponse> Error<TResponse>(string message = "")
    {
        return new Result<TResponse>
        {
            Status = ResponseStatus.Error,
            Message = message
        };
    }
}

public class Result<TResponse> : Result, IResult<TResponse>
{
    public TResponse? Response { get; set; }
}