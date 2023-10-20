using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;

namespace Chat.Framework.Models;

public class MetaDataDictionary : IMetaDataDictionary
{
    public Dictionary<string, object> MetaData { get; set; }

    public MetaDataDictionary()
    {
        MetaData = new Dictionary<string, object>();
    }

    public void SetData(string key, object data)
    {
        MetaData[key] = data;
    }

    public T? GetData<T>(string key)
    {
        if (!MetaData.TryGetValue(key, out var data)) return default;

        return data.SmartCast<T>();
    }
}