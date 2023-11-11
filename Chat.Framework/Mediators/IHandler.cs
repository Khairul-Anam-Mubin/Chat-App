namespace Chat.Framework.Mediators;

public interface IHandler {}

public interface IHandler<in TRequest> : IHandler
{
    Task HandleAsync(TRequest request);
}

public interface IHandler<in TRequest, TResponse> : IHandler
{
    Task<TResponse> HandleAsync(TRequest request);
}