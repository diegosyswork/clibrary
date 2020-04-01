
using SysWork.Data.GenericDataManager;
using SysWork.Data.GenericDataManager.Intefaces;

namespace Demo.SysWork.Data.Repositories
{
    /// <summary>
    /// Inherits of BaseGenericRepositoryManager and Implement IRepositoryManager
    /// </summary>
    public class DataManager : BaseDataManager<DataManager>, IDataManager
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
        private DataManager()
        {
        }

        /// <summary>
        /// Initializes the repositories.
        /// </summary>
        void IDataManager.InitDataObjects()
        {
            StateRepository = new StateRepository(ConnectionString, DataBaseEngine);
            PersonRepository = new PersonRepository(ConnectionString, DataBaseEngine);
            VManagerPersonsWithStates = new VManagerPersonsWithStates(ConnectionString, DataBaseEngine);
        }
    }
}
