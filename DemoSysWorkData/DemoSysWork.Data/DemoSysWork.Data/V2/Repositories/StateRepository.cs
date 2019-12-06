using Demo.SysWork.Data.Entities;
using SysWork.Data.Common;
using SysWork.Data.GenericRepostory;

namespace Demo.SysWork.Data.Repositories
{
    public class StateRepository : BaseGenericRepository<State>
    {
        public StateRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
