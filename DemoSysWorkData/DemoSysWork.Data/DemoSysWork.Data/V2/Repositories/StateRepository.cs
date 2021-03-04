using Demo.SysWork.Data.Entities;
using SysWork.Data.Common;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository;

namespace Demo.SysWork.Data.Repositories
{
    public class StateRepository : BaseRepository<State>
    {
        public StateRepository(string connectionString, EDatabaseEngine DatabaseEngine) : base(connectionString, DatabaseEngine)
        {

        }

        public State GetByStateCode(string StateCode)
        {
            State state = null;
            var resultado = GetListByLambdaExpressionFilter(entity => (entity.StateCode == StateCode));
            if (resultado != null && resultado.Count > 0)
                state = resultado[0];
            return state;
        }

    }
}
