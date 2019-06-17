using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel;
using GerdannaDataManager.Daos;
using SysWork.Data.Common;
using SysWork.Data.Common.DbConnectionUtilities;

namespace TestDaoModelDataCommon
{
    public class DataManagerTest:DataManagerBase
    {
        private static DataManagerTest _dataManagerInstance = null;

        public DaoPersona DaoPersona { get; private set; }

        private DataManagerTest()
        {
            InstanciarDaos();
        }

        public static DataManagerTest GetInstance()
        {
            ValidacionesAlCrearLaInstancia();

            if (_dataManagerInstance == null)
                _dataManagerInstance = new DataManagerTest();

            return _dataManagerInstance;
        }
        public override void InstanciarDaos()
        {
            DaoPersona = new DaoPersona(ConnectionString, DataBaseEngine);
        }

    }
}
