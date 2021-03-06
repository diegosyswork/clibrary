using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.Attributes.Helpers
{
    /// <summary>
    /// </summary>
    public static class DbColumnHelper
    {
        /// <summary>
        /// Gets the columns for select Query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public static string GetColumnsForSelect<TEntity>() where TEntity : class, new()
        {
            return GetColumnsForSelect<TEntity>(EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Sets the columns for select using an entity.
        /// The properties of entity must be have a DbColumn attribute
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public static string GetColumnsForSelect<TEntity>(EDataBaseEngine dataBaseEngine) where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            var sbColumnsSelect = new StringBuilder();
            var syntaxProvider = new SyntaxProvider(dataBaseEngine);

            foreach (PropertyInfo i in GetProperties(entity))
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                string columnName = syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

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
            return GetColumnsForInsert<TEntity>(EDataBaseEngine.MSSqlServer);
        }
        /// <summary>
        /// Gets the columns for insert.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns></returns>
        public static string GetColumnsForInsert<TEntity>(EDataBaseEngine dataBaseEngine) where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            var sbColumnsInsert = new StringBuilder();
            var syntaxProvider = new SyntaxProvider(dataBaseEngine);

            string columnName ="";
            foreach (PropertyInfo i in GetProperties(entity))
            {
                var dbColumn = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                columnName = syntaxProvider.GetSecureColumnName(dbColumn.ColumnName ?? i.Name);

                if (!dbColumn.IsIdentity)
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
            return t.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();
        }
    }
}
