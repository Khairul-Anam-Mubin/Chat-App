using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Filters;

public class UpdateDefinition : IUpdateDefinition
{
    public List<IUpdateField> Fields { get; set; }

    public UpdateDefinition(List<IUpdateField> fields)
    {
        Fields = fields;
    }

    public UpdateDefinition()
    {
        Fields = new List<IUpdateField>();
    }

    public IUpdateDefinition Add(IUpdateField field)
    {
        Fields.Add(field);
        return this;
    }

}