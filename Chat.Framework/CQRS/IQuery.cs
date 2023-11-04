using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS;

public interface IQuery : IMetaDataDictionary
{
    int Offset { get; set; }
    int Limit { get; set; }
    IQueryResponse CreateResponse();
    IQueryResponse CreateResponse(IQueryResponse response);
}