using GerdannaDataManager.Entities;
using SysWork.Data.DaoModel;
using SysWork.Data.Common;
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

        public DbExecute GetExecute()
        {
            return GetDbExecute();
        }
    }
}
