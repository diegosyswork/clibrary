using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.Common.Attributes.Helpers;
using System.Threading.Tasks;
using SysWork.Data.Mapping;

namespace SysWork.Data.Common.Mapper
{
    /// <summary>
    /// Class to map entities from a datareader. 
    /// To perform the mapping use the DbColumn <see cref="DbColumnAttribute"/>decorator.
    /// </summary>
    public class MapDataReaderToEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use type cache].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use type cache]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTypeCache { get; set; } = true;

        static Dictionary<Type, IList<PropertyInfo>> _typeCache = null;

        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IList<T> Map<T>(IDataReader reader) where T : class, new()
        {
            return Map<T>(reader, null, DefaultValues.DefaultDatabaseEngine);
        }
        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns></returns>
        public IList<T> Map<T>(IDataReader reader, EDatabaseEngine databaseEngine) where T : class, new()
        {
            return Map<T>(reader, null, databaseEngine);
        }

        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="properties">The list object property information.</param>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns></returns>
        public IList<T> Map<T>(IDataReader reader, IList<PropertyInfo> properties, EDatabaseEngine databaseEngine) where T : class, new()
        {
            T aux = new T();

            if (UseTypeCache) AddType(aux);
            var _properties = properties ??
                             (UseTypeCache ? GetCacheProperties(aux) :
                             ColumnHelper.GetProperties(aux));

            IList<T> result = new List<T>();
            while (reader.Read())
            {
                T obj = new T();

                foreach (PropertyInfo i in _properties)
                {
                    try
                    {
                        var column = (ColumnAttribute)i.GetCustomAttribute(typeof(ColumnAttribute));
                        var columnName = column.Name ?? i.Name;

                        //if ((column).Convert)
                        //{
                        //    if (reader[columnName] != DBNull.Value)
                        //        i.SetValue(obj, Convert.ChangeType(reader[columnName], i.PropertyType));
                        //}
                        //else
                        //{
                            if (reader[columnName] != DBNull.Value)
                            {
                                var value = reader[columnName];
                                var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                                var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                                i.SetValue(obj, safeValue);
                            }
                        //}
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }

                result.Add(obj);
            }
            return result;
        }
        /// <summary>
        /// Maps the specified reader asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public async Task<IList<T>> MapAsync<T>(IDataReader reader) where T : class, new()
        {
            return await MapAsync<T>(reader, null, DefaultValues.DefaultDatabaseEngine);
        }

        /// <summary>
        /// Maps the specified reader asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="databaseEngine">The database engine.</param>
        /// <returns></returns>
        public async Task<IList<T>> MapAsync<T>(IDataReader reader, EDatabaseEngine databaseEngine) where T : class, new()
        {
            return await MapAsync<T>(reader, null, databaseEngine);
        }
        /// <summary>
        /// Maps the specified reader asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="databaseEngine">The database engine.</param>
        /// <returns></returns>
        public async Task<IList<T>> MapAsync<T>(IDataReader reader, IList<PropertyInfo> properties, EDatabaseEngine databaseEngine) where T : class, new()
        {
            T aux = new T();

            if (UseTypeCache) AddType(aux);
            var _properties = properties ??
                             (UseTypeCache ? GetCacheProperties(aux) :
                             ColumnHelper.GetProperties(aux));

            IList<T> result = new List<T>();

            var TaskMapList = new Task(() =>
            {
                while (reader.Read())
                {
                    T obj = new T();
                    foreach (PropertyInfo i in _properties)
                    {
                        try
                        {
                            var column = (ColumnAttribute)i.GetCustomAttribute(typeof(ColumnAttribute));
                            var columnName = column.Name ?? i.Name;

                            //if ((column).Convert)
                            //{
                            //    if (reader[columnName] != DBNull.Value)
                            //        i.SetValue(obj, Convert.ChangeType(reader[columnName], i.PropertyType));
                            //}
                            //else
                            //{
                                if (reader[columnName] != DBNull.Value)
                                {
                                    var value = reader[columnName];
                                    var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                                    var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                                    i.SetValue(obj, safeValue);
                                }
                            //}
                        }
                        catch (Exception exception)
                        {
                            throw exception;
                        }
                    }

                    result.Add(obj);
                }
            });

            TaskMapList.Start();
            await TaskMapList;

            return result;
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            return MapSingle<TEntity>(reader, null, DefaultValues.DefaultDatabaseEngine);
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader, EDatabaseEngine databaseEngine) where TEntity : class, new()
        {
            return MapSingle<TEntity>(reader, null, databaseEngine);
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="properties">The list object property information.</param>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader, IList<PropertyInfo> properties, EDatabaseEngine databaseEngine) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            if (UseTypeCache) AddType(obj);
            var _properties = properties ??
                             (UseTypeCache ? GetCacheProperties(obj) :
                             ColumnHelper.GetProperties(obj));

            foreach (PropertyInfo i in _properties)
            {
                try
                {
                    var column = (ColumnAttribute)i.GetCustomAttribute(typeof(ColumnAttribute));
                    var columnName = column.Name ?? i.Name;

                    //if (column.Convert)
                    //{
                    //    if (reader[columnName] != DBNull.Value)
                    //        i.SetValue(obj, Convert.ChangeType(reader[columnName], i.PropertyType));
                    //}
                    //else
                    //{
                        if (reader[columnName] != DBNull.Value)
                        {
                            var value = reader[columnName];
                            var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                            var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                            i.SetValue(obj, safeValue);
                        }
                    //}
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            return obj;
        }

        /// <summary>
        /// Maps a single record DataReader asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public async Task<TEntity> MapSingleAsync<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            return await MapSingleAsync<TEntity>(reader, null, DefaultValues.DefaultDatabaseEngine);
        }
        /// <summary>
        /// Maps a single record DataReader asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="databaseEngine">The database engine.</param>
        /// <returns></returns>
        public async Task<TEntity> MapSingleAsync<TEntity>(IDataReader reader, EDatabaseEngine databaseEngine) where TEntity : class, new()
        {
            return await MapSingleAsync<TEntity>(reader, null, databaseEngine);
        }
        /// <summary>
        /// Maps a single record DataReader asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="databaseEngine">The database engine.</param>
        /// <returns></returns>
        public async Task<TEntity> MapSingleAsync<TEntity>(IDataReader reader, IList<PropertyInfo> properties, EDatabaseEngine databaseEngine) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            if (UseTypeCache) AddType(obj);
            var _properties = properties ??
                             (UseTypeCache ? GetCacheProperties(obj) :
                             ColumnHelper.GetProperties(obj));

            var taskMap = new Task(()=> 
            {
                foreach (PropertyInfo i in _properties)
                {
                    try
                    {
                        var column = (ColumnAttribute)i.GetCustomAttribute(typeof(ColumnAttribute));
                        var columnName = column.Name ?? i.Name;

                        //if (column.Convert)
                        //{
                        //    if (reader[columnName] != DBNull.Value)
                        //        i.SetValue(obj, Convert.ChangeType(reader[columnName], i.PropertyType));
                        //}
                        //else
                        //{
                            if (reader[columnName] != DBNull.Value)
                            {
                                var value = reader[columnName];
                                var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                                var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                                i.SetValue(obj, safeValue);
                            }
                        //}
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
            });

            taskMap.Start();
            await taskMap;

            return obj;
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dataRecord">The data record.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataRecord dataRecord) where TEntity : class, new()
        {
            return MapSingle<TEntity>(dataRecord, null);
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dataRecord">The data record.</param>
        /// <param name="properties">The list object property information.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataRecord dataRecord, IList<PropertyInfo> properties) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            if (UseTypeCache) AddType(obj);
            var _properties = properties ??
                             (UseTypeCache ? GetCacheProperties(obj) :
                             ColumnHelper.GetProperties(obj));

            foreach (PropertyInfo i in _properties)
            {
                try
                {
                    var column = (ColumnAttribute)i.GetCustomAttribute(typeof(ColumnAttribute));
                    var columName = column.Name ?? i.Name;

                    //if (column.Convert)
                    //{
                    //    if (dataRecord[columName] != DBNull.Value)
                    //        i.SetValue(obj, Convert.ChangeType(dataRecord[columName], i.PropertyType));
                    //}
                    //else
                    //{
                        if (dataRecord[columName] != DBNull.Value)
                        {
                            var value = dataRecord[columName];
                            var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                            var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                            i.SetValue(obj, safeValue);
                        }
                    //}
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return obj;
        }
        
        /// <summary>
        /// Maps a DataRecord asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dataRecord">The data record.</param>
        /// <returns></returns>
        public async Task<TEntity> MapSingleAsync<TEntity>(IDataRecord dataRecord) where TEntity : class, new()
        {
            return await MapSingleAsync<TEntity>(dataRecord, null);
        }
        /// <summary>
        /// Maps a DataRecord asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dataRecord">The data record.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public async Task<TEntity> MapSingleAsync<TEntity>(IDataRecord dataRecord, IList<PropertyInfo> properties) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            if (UseTypeCache) AddType(obj);
            var _properties = properties ??
                             (UseTypeCache ? GetCacheProperties(obj) :
                             ColumnHelper.GetProperties(obj));

            var taskMap = new Task(()=> 
            {
                foreach (PropertyInfo i in _properties)
                {
                    try
                    {
                        var column = (ColumnAttribute)i.GetCustomAttribute(typeof(ColumnAttribute));
                        var columName = column.Name ?? i.Name;

                        //if (column.Convert)
                        //{
                        //    if (dataRecord[columName] != DBNull.Value)
                        //        i.SetValue(obj, Convert.ChangeType(dataRecord[columName], i.PropertyType));
                        //}
                        //else
                        //{
                            if (dataRecord[columName] != DBNull.Value)
                            {
                                var value = dataRecord[columName];
                                var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                                var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                                i.SetValue(obj, safeValue);
                            }
                        //}
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
            });

            taskMap.Start();
            await taskMap;

            return obj;
        }

        /// <summary>
        /// Cleans the type of the cache.
        /// </summary>
        public void CleanCacheType()
        {
            _typeCache = new Dictionary<Type, IList<PropertyInfo>>();
        }

        private void AddType<T>(T entity)
        {
            if (_typeCache == null)
                CleanCacheType();

            Type type = entity.GetType();
            if (!_typeCache.ContainsKey(type))
                _typeCache.Add(type, ColumnHelper.GetProperties(entity));
        }

        private IList<PropertyInfo> GetCacheProperties<T>(T t)
        {
            Type type = t.GetType();
            if (_typeCache.TryGetValue(type, out IList<PropertyInfo> properties))
                return properties;
            else
                throw new IndexOutOfRangeException($"The type {type.Assembly.ToString() + type.FullName.ToString()} is not in cache");
        }
    }
}
