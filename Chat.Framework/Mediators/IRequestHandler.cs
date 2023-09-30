namespace Chat.Framework.Mediators;

public interface IRequestHandler
{

}

public interface IRequestHandler<in TRequest> : IRequestHandler
{
    Task HandleAsync(TRequest request);
}

public interface IRequestHandler<in TRequest, TResponse> : IRequestHandler
{
    Task<TResponse> HandleAsync(TRequest request);
}