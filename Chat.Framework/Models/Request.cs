using Chat.Framework.Interfaces;

namespace Chat.Framework.Models;

public class Request : IRequest
{

}

public class RequestEnvelope<TRequest> : Request, IRequest<TRequest>
{
    public TRequest? Request { get; set; }
}