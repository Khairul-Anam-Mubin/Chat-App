namespace Chat.Framework.RequestResponse;

public interface IRequest {}

public interface IRequest<TRequest> : IRequest
{
    TRequest? Request { get; set; }
}