using Chat.Framework.Models;

namespace Chat.Framework.Results;

public class Result : MetaDataDictionary, IResult
{
    public string Message { get; }
    public ResponseStatus Status { get; }

    public bool IsSuccess => Status == ResponseStatus.Success || Status == ResponseStatus.Pending;
    public bool IsFailure => !IsSuccess;

    protected Result(string message, ResponseStatus status)
    {
        Message = message;
        Status = status;
    }

    public static IResult Success(string message = "")
    {
        return new Result(message, ResponseStatus.Success);
    }

    public static IResult Error(string message = "")
    {
        return new Result(message, ResponseStatus.Error);
    }

    public static IResult Ignored(string message = "")
    {
        return new Result(message, ResponseStatus.Ignored);
    }

    public static IResult<TResponse> Success<TResponse>(TResponse? response, string message = "")
    {
        return Result<TResponse>.Create(response, message, ResponseStatus.Success);
    }

    public static IResult<TResponse> Error<TResponse>(TResponse? response, string message = "")
    {
        return Result<TResponse>.Create(response, message, ResponseStatus.Error);
    }

    public static IResult<TResponse> Success<TResponse>(string message = "")
    {
        return Result<TResponse>.Create(default, message, ResponseStatus.Success);
    }

    public static IResult<TResponse> Error<TResponse>(string message = "")
    {
        return Result<TResponse>.Create(default, message, ResponseStatus.Error);
    }
}

public class Result<TResponse> : Result, IResult<TResponse>
{
    public TResponse? Value { get; }

    private Result(TResponse? value, string message, ResponseStatus status) : base(message, status)
    {
        Value = value;
    }

    public static Result<TResponse> Create(TResponse? value, string message, ResponseStatus status)
    {
        return new Result<TResponse>(value, message, status);
    }
}