using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS
{
    public interface IQuery : IMetaDataDictionary
    {
        void ValidateQuery();
        QueryResponse CreateResponse();
        QueryResponse CreateResponse(QueryResponse response);
    }
}