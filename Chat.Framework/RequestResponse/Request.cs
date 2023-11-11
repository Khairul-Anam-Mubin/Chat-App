namespace Chat.Framework.RequestResponse;

public class Request : IRequest {}

public class RequestEnvelope<TRequest> : Request, IRequest<TRequest>
{
    public TRequest? Request { get; set; }
}