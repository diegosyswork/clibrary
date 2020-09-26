
using System;
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
        private Lazy<PersonRepository> _personRepository;
        public PersonRepository PersonRepository { get => _personRepository.Value;}
        private Lazy<StateRepository> _stateRepository;
        public StateRepository StateRepository { get => _stateRepository.Value;}

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
            _stateRepository = new Lazy<StateRepository>(()=>new StateRepository(ConnectionString, DataBaseEngine));
            _personRepository = new Lazy<PersonRepository>(() => new PersonRepository(ConnectionString, DataBaseEngine));
            VManagerPersonsWithStates = new VManagerPersonsWithStates(ConnectionString, DataBaseEngine);
        }
    }
}
