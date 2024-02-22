using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;

namespace Chat.Framework.Models;

public class MetaDataDictionary : IMetaDataDictionary
{
    public Dictionary<string, object> MetaData { get; }

    public MetaDataDictionary()
    {
        MetaData = new Dictionary<string, object>();
    }

    public bool HasKey(string key)
    {
        if (string.IsNullOrEmpty(key)) return false;

        return MetaData.ContainsKey(key);
    }

    public bool HasData(string key)
    {
        return HasKey(key) && MetaData[key] is not null;
    }

    public void SetData(string key, object data)
    {
        if (HasKey(key))
        {
            MetaData[key] = data;
            return;
        }

        MetaData.Add(key, data);
    }

    public T? GetData<T>(string key)
    {
        if (HasData(key))
        {
            return MetaData[key].SmartCast<T>();
        }

        return default;
    }
}