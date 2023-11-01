using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public interface IQueryHandler<in TQuery, TResponse> : 
    IRequestHandler<TQuery, TResponse> 
    where TQuery : class, IQuery
    where TResponse : class, IResponse {}