﻿using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.ORM.Interfaces;

public interface ISimpleFilter
{
    string FieldKey { get; set; }
    Operator Operator { get; set; }
    object FieldValue { get; set; }
}