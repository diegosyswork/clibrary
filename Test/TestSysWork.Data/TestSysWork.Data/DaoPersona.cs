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
    public class DaoPersona: BaseDao<Persona>
    {
        public DaoPersona(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        {

        }
        public Persona GetByDni(string Dni)
        {
            return GetByLambdaExpressionFilter(entity => (entity.Dni == Dni));
        }
    }
}
