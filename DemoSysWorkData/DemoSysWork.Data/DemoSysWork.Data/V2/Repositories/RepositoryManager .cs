using SysWork.Data.GenericRepositoryManager;
using SysWork.Data.GenericRepositoryManager.Intefaces;

namespace Demo.SysWork.Data.Repositories
{
    /// <summary>
    /// Inherits of BaseGenericRepositoryManager and Implement IRepositoryManager
    /// </summary>
    public class RepositoryManager : BaseRepositoryManager<RepositoryManager> , IRepositoryManager
    {
        /// <summary>
        /// Add Repositories to Manage.
        /// </summary>
        public PersonRepository PersonRepository {get; private set;}
        public StateRepository StateRepository {get; private set;}

        public VManagerPersonsWithStates VManagerPersonsWithStates { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="RepositoryManager"/> class from being created.
        /// </summary>
        private RepositoryManager()
        {
        }

        /// <summary>
        /// Initializes the repositories.
        /// </summary>
        void IRepositoryManager.InitRepositories()
        {
            StateRepository = new StateRepository(ConnectionString, DataBaseEngine);
            PersonRepository = new PersonRepository(ConnectionString, DataBaseEngine);
            VManagerPersonsWithStates = new VManagerPersonsWithStates(ConnectionString, DataBaseEngine);
        }
    }
}
