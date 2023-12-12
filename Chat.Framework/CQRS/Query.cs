using Chat.Framework.Mediators;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public interface IQuery {}

public interface IQueryHandler<in TQuery, TResponse> : IHandler<TQuery, IResult<TResponse>>
    where TQuery : class, IQuery 
    where TResponse : class {}