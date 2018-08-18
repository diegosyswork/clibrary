using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel.Attributes;
using GerdannaDataManager.Entities;
using SysWork.Data.DaoModel;
using SysWork.Data.Common;
using System.Data.Common;
using System.Data;

namespace GerdannaDataManager.Daos
{
    public class DaoPersonaSqlite : BaseDao<Persona>
    {
        /// <summary>
        /// **********************************************************************
        /// 
        /// Esta clase fue generada automaticamente por la clase DaoClassFromDb
        /// 
        /// Fecha: 14/08/2018 17:31:01
        /// 
        /// **********************************************************************
        /// </summary>

        public DaoPersonaSqlite(string connectionString) : base(connectionString, EDataBaseEngine.SqLite)
        {

        }

        public Persona GetByDni(string Dni)
        {
            Persona persona = null;
            var resultado = GetListByLambdaExpressionFilter(entity => (entity.Dni == Dni));
            if (resultado != null && resultado.Count > 0)
                persona = resultado[0];
            return persona;
        }

        public DbConnection GetConnection()
        {
            return GetDbConnection();
        }
    }
    public class DaoPersonaSql : BaseDao<Persona>
    {
        /// <summary>
        /// **********************************************************************
        /// 
        /// Esta clase fue generada automaticamente por la clase DaoClassFromDb
        /// 
        /// Fecha: 14/08/2018 17:31:01
        /// 
        /// **********************************************************************
        /// </summary>

        public DaoPersonaSql(string connectionString) : base(connectionString, EDataBaseEngine.MSSqlServer)
        {

        }

        public Persona GetByDni(string Dni)
        {
            Persona persona = null;
            var resultado = GetListByLambdaExpressionFilter(entity => (entity.Dni == Dni));
            if (resultado != null && resultado.Count > 0)
                persona = resultado[0];
            return persona;
        }
        public DbConnection GetConnection()
        {
            return GetDbConnection();
        }

    }
    public class DaoPersonaOleDb: BaseDao<Persona>
    {
        /// <summary>
        /// **********************************************************************
        /// 
        /// Esta clase fue generada automaticamente por la clase DaoClassFromDb
        /// 
        /// Fecha: 14/08/2018 17:31:01
        /// 
        /// **********************************************************************
        /// </summary>

        public DaoPersonaOleDb(string connectionString) : base(connectionString, EDataBaseEngine.OleDb)
        {

        }

        public Persona GetByDni(string Dni)
        {
            Persona persona = null;
            var resultado = GetListByLambdaExpressionFilter(entity => (entity.Dni == Dni));
            if (resultado != null && resultado.Count > 0)
                persona = resultado[0];
            return persona;
        }
        public DbConnection GetConnection()
        {
            return GetDbConnection();
        }
        public IDbConnection Get_IDBConnection()
        {
            return GetIDbConnection();
        }
        public IDbCommand Get_IDBCommand()
        {
            return GetIDbCommand();
        }

        public DbConnection Get_PersistentConnection()
        {
            return GetPersistentDbConnection();
        }

    }
}
