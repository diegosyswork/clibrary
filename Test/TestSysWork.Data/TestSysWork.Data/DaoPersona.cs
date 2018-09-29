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
using SysWork.Data.Common.DbConnectionUtilities;

namespace GerdannaDataManager.Daos
{
    public class DaoPersonaSqlite : BaseDao<Persona>
    {
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

    }

    public class DaoPersonaSql : BaseDao<Persona>
    {
 
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

    }
    public class DaoPersonaOleDb : BaseDao<Persona>
    {
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
    }
    public class DaoPersonaMySql : BaseDao<Persona>
    {
        public DaoPersonaMySql(string connectionString) : base(connectionString, EDataBaseEngine.MySql)
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

    }
}
