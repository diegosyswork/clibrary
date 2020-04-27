using GerdannaDataManager.Entities;
using SysWork.Data.Common;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository;

namespace GerdannaDataManager.Daos
{
    public class DaoPersona: BaseRepository<Persona>
    {
        public DaoPersona(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        {

        }
        public Persona GetByDni(string Dni)
        {
            return GetByLambdaExpressionFilter(entity => (entity.Dni == Dni));
        }

        public DbExecutor GetDBExecute()
        {
            return BaseDbExecutor();
        }
    }
}
