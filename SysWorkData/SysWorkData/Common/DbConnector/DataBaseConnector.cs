using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// 
    /// </summary>
    public class DataBaseConnector
    {
        private AbstractDbConnector _dbObjectConnector;
        private EDataBaseEngine _dataBaseEngine { get; set; }

        /// <summary>
        /// Devuelve o Establece el nombre de la cadena de conexion dentro del archivos de configuracion
        /// <see cref="WriteInConfigFile"/>
        /// <see cref="TryGetConnectionStringFromConfig"/>
        /// </summary>
        public string ConnectionStringName { get { return _dbObjectConnector.ConnectionStringName; } set { _dbObjectConnector.ConnectionStringName = value; } }

        /// <summary>
        /// Devuelve o Establece la ConnectionString del Connector
        /// </summary>
        public string ConnectionString { get { return _dbObjectConnector.ConnectionString; } set { _dbObjectConnector.ConnectionString = value; } }

        /// <summary>
        /// Determina si en caso que la cadena de conexion este vacia, o no se pueda realizar la conexion, el usuario
        /// podrá informar los parametros de conexion.
        /// </summary>
        public bool PromptUser { get { return _dbObjectConnector.PromptUser; } set { _dbObjectConnector.PromptUser = value; } }

        /// <summary>
        /// Devuelve si el usuario, ingreso datos.
        /// </summary>
        public bool UserGotParameters { get { return _dbObjectConnector.UserGotParameters; } private set { } } 

        /// <summary>
        /// Origen de datos Predeterminado (nombre de server / instancia)
        /// </summary>
        public string DefaultDataSource { get { return _dbObjectConnector.DefaultDataSource ; } set { _dbObjectConnector.DefaultDataSource = value; } } 

        /// <summary>
        /// Login Predeterminado
        /// </summary>
        public string DefaultUser { get { return _dbObjectConnector.DefaultUser; } set { _dbObjectConnector.DefaultUser = value;}} 

        /// <summary>
        /// Password Predeterminado
        /// </summary>
        public string DefaultPassword { get { return _dbObjectConnector.DefaultPassword; } set { _dbObjectConnector.DefaultPassword = value; } } 

        /// <summary>
        /// Base de datos predeterminada.
        /// </summary>
        public string DefaultDatabase { get { return _dbObjectConnector.DefaultDatabase; } set { _dbObjectConnector.DefaultDatabase = value; } }

        /// <summary>
        /// Devuelve si se pudo realizar la conexion correctamente con los datos proporcionados.
        /// </summary>
        public bool IsConnectionSuccess { get { return _dbObjectConnector.IsConnectionSuccess; } private set { } } 

        /// <summary>
        /// En caso de conexion correcta escribe la cadena de conexion en el archivo de configuracion
        /// </summary>
        public bool WriteInConfigFile { get { return _dbObjectConnector.WriteInConfigFile; } set { _dbObjectConnector.WriteInConfigFile = value; } }

        /// <summary>
        /// Devuelve o Establece si se debe encriptar datos sensibles de la cadena de conexion, como ser el nombre
        /// de usuario, base de datos y password.
        /// </summary>
        public bool IsEncryptedData { get { return _dbObjectConnector.IsEncryptedData; } set { _dbObjectConnector.IsEncryptedData = value; } } 

        /// <summary>
        /// En el caso que se informe un ConnectionStringName determina si se intenta obtener
        /// desde el archivo de configuracion.
        /// <seealso cref="ConnectionStringName"/>
        /// </summary>
        public bool TryGetConnectionStringFromConfig{ get { return _dbObjectConnector.TryGetConnectionStringFromConfig; } set { _dbObjectConnector.TryGetConnectionStringFromConfig = value; } } 

        /// <summary>
        /// Muestra el ultimo mensaje de error ocurrido al intentar abrir la conecction.
        /// </summary>
        public string ConnectionError{get { return _dbObjectConnector.ConnectionError; } private set { } }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseConnector"/> class.
        /// </summary>
        public DataBaseConnector()
        {
            ResolveConstructor(EDataBaseEngine.MSSqlServer);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseConnector"/> class.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public DataBaseConnector(EDataBaseEngine dataBaseEngine)
        {
            ResolveConstructor(dataBaseEngine);
        }

        private void ResolveConstructor(EDataBaseEngine dataBaseEngine)
        {
            _dataBaseEngine = dataBaseEngine;
            switch (_dataBaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    _dbObjectConnector = new DbConnectorMSSqlServer();
                    break;
                case EDataBaseEngine.SqLite:
                    _dbObjectConnector = new DbConnectorSqLite();
                    break;
                case EDataBaseEngine.OleDb:
                    _dbObjectConnector = new DbConnectorOleDb();
                    break;
                case EDataBaseEngine.MySql:
                    _dbObjectConnector = new DbConnectorMySql();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("El motor de base de datos informado no esta admitido para esta clase");
            }
        }
        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            _dbObjectConnector.Connect();
        }
    }
}
