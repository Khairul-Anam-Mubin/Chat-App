using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS;

public interface IQuery : IMetaDataDictionary
{
    IQueryResponse CreateResponse();
    IQueryResponse CreateResponse(IQueryResponse response);
}