﻿using Chat.Activity.Domain.Entities;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Activity.Infrastructure.Migrations;

public class PresenceIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public PresenceIndexCreator(DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        _indexManager.CreateOne<Presence>(_databaseInfo,
            new IndexBuilder<Presence>()
                .Ascending(o => o.UserId)
                .Build());
    }
}