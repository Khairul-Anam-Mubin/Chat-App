namespace Chat.Framework.Interfaces;

public interface IMetaDataDictionary
{
    Dictionary<string, object> MetaData { get; }
    bool HasKey(string key);
    bool HasData(string key);
    void SetData(string key, object value);
    T? GetData<T>(string key);
}