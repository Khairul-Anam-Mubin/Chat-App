using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Database.ORM.Sql.Composers;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Chat.Framework.Database.ORM.Sql
{
    public class SqlDbContext : IDbContext
    {
        public Dictionary<string, object> GetPropertyValueDictionary(object? pObject)
        {
            var propertyValueDictionary = new Dictionary<string, object>();

            if (pObject == null) return propertyValueDictionary;
            
            foreach (var prop in pObject.GetType().GetProperties())
            {
                propertyValueDictionary.TryAdd(prop.Name, prop.GetValue(pObject));
            }
            return propertyValueDictionary;
        }

        private IDbConnection CreateConnection(DatabaseInfo databaseInfo)
        {
            return new SqlConnection(databaseInfo.ConnectionString);
        }

        public async Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IEntity
        {
            try
            {
                var tableName = typeof(T).Name;

                var query = $"INSERT INTO {tableName}";

                var propertyValueDictionary = GetPropertyValueDictionary(item);

                query += " (";

                int cnt = 0;

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

                var dynamicParameters = new Dictionary<string, object>();

                foreach (var kv in propertyValueDictionary)
                {
                    dynamicParameters.TryAdd(kv.Key, kv.Value);
                }

                using var connection = CreateConnection(databaseInfo);

                var rowsAffected = await connection.ExecuteAsync(query, new DynamicParameters(dynamicParameters));

                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public async Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IEntity
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

        public async Task<bool> UpdateOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IEntity
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

        public async Task<bool> UpdateManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

                var tableName = typeof(T).Name;

                var filterQuery = new SqlDbFilterComposer().Compose(filter);

                var updateQuery = new SqlDbUpdateComposer().Compose(update);

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

        public async Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity
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

        public async Task<bool> DeleteManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

                var tableName = typeof(T).Name;

                var query = $"DELETE FROM {tableName}";

                var filterQuery = new SqlDbFilterComposer().Compose(filter);

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

        public async Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity
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

        public async Task<T?> GetOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

                var tableName = typeof(T).Name;

                var filterQuery = new SqlDbFilterComposer().Compose(filter);

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

        public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity
        {
            try
            {
                var tableName = typeof(T).Name;

                var query = $"SELECT * FROM {tableName}";
                
                using var connection = CreateConnection(databaseInfo);

                var entities = await connection.QueryAsync<T>(query);
                
                return entities.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<T>();
        }

        public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

                var tableName = typeof(T).Name;

                var filterQuery = new SqlDbFilterComposer().Compose(filter);

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

        public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, ISort sort, int offset, int limit) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

                var tableName = typeof(T).Name;

                var filterQuery = new SqlDbFilterComposer().Compose(filter);

                var sortQuery = new SqlDbSortComposer().Compose(sort);

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

        public async Task<long> CountAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

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

        public async Task<long> CountAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
        {
            try
            {
                using var connection = CreateConnection(databaseInfo);

                var tableName = typeof(T).Name;

                var filterQuery = new SqlDbFilterComposer().Compose(filter);

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
}
