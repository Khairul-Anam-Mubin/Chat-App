using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.ORM.Filters;

public class Update : IUpdate
{
    public List<IUpdateField> Fields { get; set; }

    public Update(List<IUpdateField> fields)
    {
        Fields = fields;
    }

    public Update()
    {
        Fields = new List<IUpdateField>();
    }

    public IUpdate Add(IUpdateField field)
    {
        Fields.Add(field);
        return this;
    }

}