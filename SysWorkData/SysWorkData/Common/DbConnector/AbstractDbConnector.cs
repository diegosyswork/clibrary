using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Data.Common.DbConnector
{

    /// <summary>
    /// Abstract class to implement an DatabaseConnector.
    /// </summary>
    public abstract class AbstractDbConnector
    {
        /// <summary>
        /// Determine the name of the connectionString in case it is written to the app configuration file
        /// <see cref="WriteInConfigFile"/>
        /// <see cref="TryGetConnectionStringFromConfig"/>
        /// </summary>
        public string ConnectionStringName { get; set; } = string.Empty;

        /// <summary>
        /// gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set;}

        /// <summary>
        /// Determines if in case the connection fails, the user connection parameters will be requested
        /// </summary>
        /// <value>
        ///   <c>true</c> if [prompt user]; otherwise, <c>false</c>.
        /// </value>
        public bool PromptUser { get; set; } = true;

        /// <summary>
        /// Get if the user informed the parameters.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [user got parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool UserGotParameters { get; protected set; } = false;

        /// <summary>
        /// Gets or sets the default data source (SERVER / Instance). this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default data source.
        /// </value>
        public string DefaultDataSource { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the default user. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default user.
        /// </value>
        public string DefaultUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the default password. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default password.
        /// </value>
        public string DefaultPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the default database. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default database.
        /// </value>
        public string DefaultDatabase { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value that indicates whether the connection has been established correctly.
        /// </summary>
        /// <value>
        /// </value>
        public bool IsConnectionSuccess { get; protected set; } = false;

        /// <summary>
        /// Determine if it should be written to the configuration file. It will be written only if the connection was successful.
        /// </summary>
        public bool WriteInConfigFile { get; set; } = false;

        /// <summary>
        /// Determines whether the parameters must be encrypted / decrypted.
        /// </summary>
        public bool IsEncryptedData { get; set; } = false;

        /// <summary>
        /// Try to get the connection string from the app configuration file. Need a ConnectionStringName
        /// <seealso cref="ConnectionStringName"/>
        /// </summary>
        public bool TryGetConnectionStringFromConfig { get; set; } = false;
        
        /// <summary>
        /// Gets the last error message.
        /// </summary>
        public string ConnectionError { get; protected set; } = string.Empty;

        /// <summary>
        /// try to connect to the specified parameters.
        /// </summary>
        public abstract void Connect();
    }
}
