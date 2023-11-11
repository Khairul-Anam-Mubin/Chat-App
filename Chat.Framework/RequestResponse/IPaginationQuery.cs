using Chat.Framework.Interfaces;

namespace Chat.Framework.RequestResponse;

public interface IPaginationQuery : IMetaDataDictionary
{
    int Offset { get; set; }
    int Limit { get; set; }

    IPaginationResponse CreateResponse();
}

public interface IPaginationQuery<TItem> : IMetaDataDictionary
{
    int Offset { get; set; }
    int Limit { get; set; }

    IPaginationResponse<TItem> CreateResponse();
}