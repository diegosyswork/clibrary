using System;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    ///  Abstract Implementation of IDbConnector.
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DbConnector.IDbConnector" />
    public abstract class AbstractDBConnector : IDbConnector
    {
        /// <summary>
        /// Determine the name of the connectionString in case it is written to the app configuration file
        /// <see cref="WriteInConfigFile"/>
        /// <see cref="TryGetConnectionStringFromConfig"/>
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Determines if in case the connection fails, the user connection parameters will be requested
        /// </summary>
        /// <value>
        ///   <c>true</c> if [prompt user]; otherwise, <c>false</c>.
        /// </value>
        public bool PromptUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [before connect show defaults parameters].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [before connect show defaults parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool BeforeConnectShowDefaultsParameters { get; set; }


        /// <summary>
        /// Get if the user informed the parameters.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [user got parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool UserGotParameters { get; set; }

        /// <summary>
        /// Gets or sets the default data source (SERVER / Instance). this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default data source.
        /// </value>
        public string DefaultDataSource { get; set; }

        /// <summary>
        /// Gets or sets the default user. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default user.
        /// </value>
        public string DefaultUser { get; set; }

        /// <summary>
        /// Gets or sets the default password. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default password.
        /// </value>
        public string DefaultPassword { get; set; }

        /// <summary>
        /// Gets or sets the default database. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default database.
        /// </value>
        public string DefaultDatabase { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the connection has been established correctly.
        /// </summary>
        /// <value>
        /// </value>
        public bool IsConnectionSuccess { get; set; }

        /// <summary>
        /// Determine if it should be written to the configuration file. It will be written only if the connection was successful.
        /// </summary>
        public bool WriteInConfigFile { get; set; }

        /// <summary>
        /// Determines whether the parameters must be encrypted / decrypted.
        /// </summary>
        public bool IsEncryptedData { get; set; }

        /// <summary>
        /// Try to get the connection string from the app configuration file. Need a ConnectionStringName
        /// <seealso cref="ConnectionStringName"/>
        /// </summary>
        public bool TryGetConnectionStringFromConfig { get; set; }

        /// <summary>
        /// Gets the last error message.
        /// </summary>
        public string ConnectionError { get; set; }

        /// <summary>
        /// Gets or sets the connector parameter type used.
        /// </summary>
        /// <value>
        /// The connector parameter type used.
        /// </value>
        public EConnectorParameterTypeUsed ConnectorParameterTypeUsed { get; set; }

        /// <summary>
        /// try to connect to the specified parameters.
        /// </summary>
        public virtual void Connect()
        {
            throw new NotImplementedException();
        }
    }
}
