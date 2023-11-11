namespace Chat.Framework.Interfaces;

public interface IRequest
{ 

}

public interface IRequest<TRequest> : IRequest
{
    TRequest? Request { get; set; }
}