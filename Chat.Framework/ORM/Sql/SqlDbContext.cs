using Chat.Framework.Extensions;
using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.ORM.Sql.Composers;
using Dapper;

namespace Chat.Framework.ORM.Sql;

public class SqlDbContext : IDbContext
{
    private readonly ISqlClientManager _clientManager;

    public SqlDbContext(ISqlClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    public async Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
    {
        try
        {
            var tableName = typeof(T).Name;

            var query = $"INSERT INTO {tableName}";

            var propertyValueDictionary = item.ToDictionary();

            query += " (";

            var cnt = 0;

            foreach (var kv in propertyValueDictionary)
            {
                if (cnt + 1 != propertyValueDictionary.Count)
                {
                    query += $"{kv.Key}, ";
                }
                else
                {
                    query += $"{kv.Key})";
                }
                cnt++;
            }

            query += " VALUES (";

            cnt = 0;

            foreach (var kv in propertyValueDictionary)
            {
                if (cnt + 1 != propertyValueDictionary.Count)
                {
                    query += $"@{kv.Key}, ";
                }
                else
                {
                    query += $"@{kv.Key})";
                }
                cnt++;
            }

            using var connection = _clientManager.CreateConnection(databaseInfo);

            var rowsAffected =
                await connection.ExecuteAsync(query, new DynamicParameters(propertyValueDictionary));

            return rowsAffected > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IRepositoryItem
    {
        try
        {
            foreach (var item in items)
            {
                await SaveAsync(databaseInfo, item);
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<bool> UpdateOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IRepositoryItem
    {
        try
        {
            return await UpdateManyAsync<T>(databaseInfo, filter, update);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<bool> UpdateManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var composer = new SqlDbComposerFacade();

            var filterQuery = composer.Compose(filter);

            var updateQuery = composer.Compose(update);

            var query = $"UPDATE {tableName}";

            if (!string.IsNullOrEmpty(updateQuery.Query))
            {
                query += $" SET {updateQuery.Query}";
            }

            if (!string.IsNullOrEmpty(filterQuery.Query))
            {
                query += $" WHERE {filterQuery.Query}";
            }

            var rowsAffected = await connection.ExecuteAsync(query, new DynamicParameters(filterQuery.MergeQueryParameters(updateQuery.DynamicParameters).DynamicParameters));

            return rowsAffected > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
    {
        try
        {
            var filter = new FilterBuilder<T>().Eq(o => o.Id, id);

            return await DeleteManyAsync<T>(databaseInfo, filter);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    public async Task<bool> DeleteManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var query = $"DELETE FROM {tableName}";

            var filterQuery = new SqlDbComposerFacade().Compose(filter);

            if (!string.IsNullOrEmpty(filterQuery.Query))
            {
                query += $" WHERE {filterQuery.Query}";
            }

            var entity = await connection.ExecuteAsync(query, new DynamicParameters(filterQuery.DynamicParameters));

            return entity > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return default;
    }

    public async Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
    {
        try
        {
            var filter = new FilterBuilder<T>().Eq(o => o.Id, id);

            return await GetOneAsync<T>(databaseInfo, filter);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return default;
    }

    public async Task<T?> GetOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var filterQuery = new SqlDbComposerFacade().Compose(filter);

            var query = $"SELECT * FROM {tableName}";

            if (!string.IsNullOrEmpty(filterQuery.Query))
            {
                query += $" WHERE {filterQuery.Query}";
            }

            var entity = await connection.QuerySingleOrDefaultAsync<T>(query, new DynamicParameters(filterQuery.DynamicParameters));

            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return default;
    }

    public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem
    {
        try
        {
            var tableName = typeof(T).Name;

            var query = $"SELECT * FROM {tableName}";

            using var connection = _clientManager.CreateConnection(databaseInfo);

            var entities = await connection.QueryAsync<T>(query);

            return entities.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return new List<T>();
    }

    public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var filterQuery = new SqlDbComposerFacade().Compose(filter);

            var query = $"SELECT * FROM {tableName}";

            if (!string.IsNullOrEmpty(filterQuery.Query))
            {
                query += $" WHERE {filterQuery.Query}";
            }

            var entity = await connection.QueryAsync<T>(query, new DynamicParameters(filterQuery.DynamicParameters));

            return entity.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return new List<T>();
    }

    public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, ISort sort, int offset, int limit) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var composer = new SqlDbComposerFacade();

            var filterQuery = composer.Compose(filter);

            var sortQuery = composer.Compose(sort);

            var query = $"SELECT * FROM {tableName}";

            if (!string.IsNullOrEmpty(filterQuery.Query))
            {
                query += $" WHERE {filterQuery.Query}";
            }

            if (!string.IsNullOrEmpty(sortQuery))
            {
                query += $" ORDER BY {sortQuery}";
            }

            query += $" LIMIT {limit} OFFSET {offset}";

            var entity = await connection.QueryAsync<T>(query, new DynamicParameters(filterQuery.DynamicParameters));

            return entity.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return new List<T>();
    }

    public async Task<long> CountAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var query = $"SELECT COUNT(*) FROM {tableName}";

            var count = await connection.ExecuteScalarAsync<long>(query);

            return count;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return 0;
    }

    public async Task<long> CountAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem
    {
        try
        {
            using var connection = _clientManager.CreateConnection(databaseInfo);

            var tableName = typeof(T).Name;

            var filterQuery = new SqlDbComposerFacade().Compose(filter);

            var query = $"SELECT COUNT(*) FROM {tableName}";

            if (!string.IsNullOrEmpty(filterQuery.Query))
            {
                query += $" WHERE {filterQuery.Query}";
            }

            var count = await connection.ExecuteScalarAsync<long>(query, new DynamicParameters(filterQuery.DynamicParameters));

            return count;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return 0;
    }
}