using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS;

public interface IQuery : IMetaDataDictionary
{
    QueryResponse CreateResponse();
    QueryResponse CreateResponse(QueryResponse response);
}