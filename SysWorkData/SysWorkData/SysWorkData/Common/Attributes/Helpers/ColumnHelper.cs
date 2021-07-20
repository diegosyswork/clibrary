using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Mapping;

namespace SysWork.Data.Common.Attributes.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ColumnHelper
    {
        /// <summary>
        /// Gets the columns for select Query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public static string GetColumnsForSelect<TEntity>() where TEntity : class, new()
        {
            return GetColumnsForSelect<TEntity>(EDatabaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Sets the columns for select using an entity.
        /// The properties of entity must be have a DbColumn attribute
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public static string GetColumnsForSelect<TEntity>(EDatabaseEngine databaseEngine) where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            var sbColumnsSelect = new StringBuilder();
            var syntaxProvider = new SyntaxProvider(databaseEngine);

            foreach (PropertyInfo i in GetProperties(entity))
            {
                var customAttribute = i.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                string columnName = syntaxProvider.GetSecureColumnName(customAttribute.Name ?? i.Name);

                sbColumnsSelect.Append(string.Format("{0},", columnName));
            }

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            return sbColumnsSelect.ToString();
        }

        /// <summary>
        /// Gets the columns for insert.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public static string GetColumnsForInsert<TEntity>() where TEntity : class, new()
        {
            return GetColumnsForInsert<TEntity>(EDatabaseEngine.MSSqlServer);
        }
        /// <summary>
        /// Gets the columns for insert.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns></returns>
        public static string GetColumnsForInsert<TEntity>(EDatabaseEngine databaseEngine) where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            var sbColumnsInsert = new StringBuilder();
            var syntaxProvider = new SyntaxProvider(databaseEngine);

            string columnName;
            foreach (PropertyInfo i in GetProperties(entity))
            {
                var dbColumn = i.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                columnName = syntaxProvider.GetSecureColumnName(dbColumn.Name ?? i.Name);

                if (!dbColumn.IsGenerated)
                    sbColumnsInsert.Append(string.Format("{0},", columnName));
            }

            if (sbColumnsInsert.Length > 0)
                sbColumnsInsert.Remove(sbColumnsInsert.Length - 1, 1);

            return sbColumnsInsert.ToString();
        }

        /// <summary>
        /// Gets the entity properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static  IList<PropertyInfo> GetProperties<T>(T t)
        {
            return t.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), true).Count() != 0).ToList();
            //return t.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(ColumnAttribute)) != null).ToList(); 
        }
    }
}
