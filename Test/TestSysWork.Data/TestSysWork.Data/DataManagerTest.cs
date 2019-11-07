using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel;
using GerdannaDataManager.Daos;
using SysWork.Data.Common;
using SysWork.Data.Common.DbConnectionUtilities;
using System.Data.SqlClient;

namespace TestDaoModelDataCommon
{
    public class DataManagerTest:DataManagerBase
    {
        private static DataManagerTest _dataManagerInstance = null;

        public DaoPersona DaoPersona { get; private set; }

        private DataManagerTest()
        {
            InitDAOS();
        }

        public static DataManagerTest GetInstance()
        {
            ValidateInstanceCreation();

            if (_dataManagerInstance == null)
                _dataManagerInstance = new DataManagerTest();

            return _dataManagerInstance;
        }

        public override void InitDAOS()
        {
            DaoPersona = new DaoPersona(ConnectionString, DataBaseEngine);
        }
    }
}
