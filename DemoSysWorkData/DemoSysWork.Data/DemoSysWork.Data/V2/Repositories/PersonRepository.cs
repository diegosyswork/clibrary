using Demo.SysWork.Data.Entities;
using System.Data.Common;
using SysWork.Data.Common;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepostory;

namespace Demo.SysWork.Data.Repositories
{
    public class PersonRepository : BaseGenericRepository<Person>
    {
        public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        {
        }

        public Person GetByPassport(string Passport)
        {
            Person persons = null;
            var resultado = GetListByLambdaExpressionFilter(entity => (entity.Passport == Passport));
            if (resultado != null && resultado.Count > 0)
                persons = resultado[0];
            return persons;
        }

        public DbExecutor GetDbExecutor()
        {
            return BaseDbExecutor();
        }

        public DbConnection GetDbConnection()
        {
            return BaseDbConnection();
        }
    }
}
