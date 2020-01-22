using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using SysWork.Data.Common;
using SysWork.Data.Common.Syntax;
using SysWork.Data.GenericRepository.Attributes;

namespace SysWork.Data.GenericRepository.Mapper
{
    ///TODO: Revisar la documentacion y crear ejemplo del uso de los metodos.
    /// <summary>
    /// Class to map entities from a datareader. 
    /// To perform the mapping use the DbColumn <see cref="DbColumnAttribute"/>decorator.
    /// </summary>
    public class MapDataReaderToEntity
    {
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IList<T> Map<T>(IDataReader reader) where T : class, new()
        {
            return Map<T>(reader, null, EDataBaseEngine.MSSqlServer);
        }
        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns></returns>
        public IList<T> Map<T>(IDataReader reader, EDataBaseEngine dataBaseEngine) where T : class, new()
        {
            return Map<T>(reader, null, dataBaseEngine);
        }

        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="listObjectPropertyInfo">The list object property information.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns></returns>
        public IList<T> Map<T>(IDataReader reader, IList<PropertyInfo> listObjectPropertyInfo, EDataBaseEngine dataBaseEngine) where T : class, new()
        {
            _syntaxProvider = new SyntaxProvider(dataBaseEngine);

            T obj = new T();

            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            IList<T> collection = new List<T>();
           
            while (reader.Read())
            {
                obj = new T();

                foreach (PropertyInfo i in _propertyInfo)
                {
                    try
                    {
                        var custumAttribute = (DbColumnAttribute)i.GetCustomAttribute(typeof(DbColumnAttribute));
                        var columnName = custumAttribute.ColumnName ?? i.Name;

                        if ((custumAttribute).Convert)
                        {
                            if (reader[columnName] != DBNull.Value)
                                i.SetValue(obj, Convert.ChangeType(reader[columnName], i.PropertyType));
                        }
                        else
                        {
                            if (reader[columnName] != DBNull.Value)
                            {
                                var value = reader[columnName];
                                var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                                var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                                i.SetValue(obj, safeValue);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
                collection.Add(obj);
            }
            return collection;
        }
        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            return MapSingle<TEntity>(reader, null, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader, EDataBaseEngine dataBaseEngine) where TEntity : class, new()
        {
            return MapSingle<TEntity>(reader, null, dataBaseEngine);
        }

        /// <summary>
        /// Maps the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="listObjectPropertyInfo">The list object property information.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader, IList<PropertyInfo> listObjectPropertyInfo, EDataBaseEngine dataBaseEngine) where TEntity : class, new()
        {
            _syntaxProvider = new SyntaxProvider(dataBaseEngine);

            TEntity obj = new TEntity();
            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            //obj = default(TEntity);
            //if (reader.Read())
            //{
            obj = new TEntity();
            foreach (PropertyInfo i in _propertyInfo)
            {
                try
                {
                    var custumAttribute = (DbColumnAttribute)i.GetCustomAttribute(typeof(DbColumnAttribute));
                    var columnName = custumAttribute.ColumnName ?? i.Name;

                    if (custumAttribute.Convert)
                    {
                        if (reader[columnName] != DBNull.Value)
                            i.SetValue(obj, Convert.ChangeType(reader[columnName], i.PropertyType));
                    }
                    else
                    {
                        if (reader[columnName] != DBNull.Value)
                        {
                            var value = reader[columnName];
                            var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                            var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                            i.SetValue(obj, safeValue);
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            //}
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
        /// <param name="listObjectPropertyInfo">The list object property information.</param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataRecord dataRecord, IList<PropertyInfo> listObjectPropertyInfo) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            obj = new TEntity();
            foreach (PropertyInfo i in _propertyInfo)
            {
                try
                {
                    var custumAttribute = (DbColumnAttribute)i.GetCustomAttribute(typeof(DbColumnAttribute));
                    var columName = custumAttribute.ColumnName ?? i.Name;

                    if (custumAttribute.Convert)
                    {
                        if (dataRecord[columName] != DBNull.Value)
                            i.SetValue(obj, Convert.ChangeType(dataRecord[columName], i.PropertyType));
                    }
                    else
                    {
                        if (dataRecord[columName] != DBNull.Value)
                        {
                            var value = dataRecord[columName];
                            var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                            var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                            i.SetValue(obj, safeValue);
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return obj;
        }
    }
}
