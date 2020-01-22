using Demo.SysWork.Data.Entities;
using System.Data.Common;
using SysWork.Data.Common;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepository;

namespace Demo.SysWork.Data.Repositories
{
    public class PersonRepository : BaseRepository<Person>
    {
        public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        {
        }

        public Person GetByPassport(string Passport)
        {
            return GetByLambdaExpressionFilter(entity => (entity.Passport == Passport));
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
