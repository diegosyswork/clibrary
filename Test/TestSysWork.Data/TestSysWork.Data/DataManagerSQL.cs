﻿using System;
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
    public class DataManagerSQL 
    {
        private static string _connectionString;
        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }

        private static EDataBaseEngine _dataBaseEngine;
        public static EDataBaseEngine DataBaseEngine { get { return _dataBaseEngine; } set { _dataBaseEngine = value; } }

        protected static void ValidacionesAlCrearLaInstancia()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("No se ha informado la cadena de conexion del DataManager");
        }

        private static DataManagerSQL _dataManagerInstance = null;

        public DaoPersona DaoPersonaSql { get; private set; }

        private DataManagerSQL()
        {
            InstanciarDaos();
        }

        public static DataManagerSQL GetInstance()
        {
            ValidacionesAlCrearLaInstancia();

            if (_dataManagerInstance == null)
                _dataManagerInstance = new DataManagerSQL();

            return _dataManagerInstance;
        }
        public void InstanciarDaos()
        {
            DaoPersonaSql = new DaoPersona(ConnectionString, DataBaseEngine);
        }
        public DbExecute GetDbExecute()
        {
            return new DbExecute(_connectionString, _dataBaseEngine);
        }

    }
}
