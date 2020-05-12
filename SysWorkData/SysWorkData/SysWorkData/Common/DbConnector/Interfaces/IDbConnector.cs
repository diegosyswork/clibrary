using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Interface to implement an DatabaseConnector.
    /// </summary>
    public interface IDbConnector
    {
        /// <summary>
        /// Determine the name of the connectionString in case it is written to the app configuration file
        /// <see cref="WriteInConfigFile"/>
        /// <see cref="TryGetConnectionStringFromConfig"/>
        /// </summary>
        string ConnectionStringName { get; set; } 

        /// <summary>
        /// gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; set;}

        /// <summary>
        /// Determines if in case the connection fails, the user connection parameters will be requested
        /// </summary>
        /// <value>
        ///   <c>true</c> if [prompt user]; otherwise, <c>false</c>.
        /// </value>
        bool PromptUser { get; set; } 

        /// <summary>
        /// Gets or sets a value indicating whether [before connect show defaults parameters].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [before connect show defaults parameters]; otherwise, <c>false</c>.
        /// </value>
        bool BeforeConnectShowDefaultsParameters { get; set; } 


        /// <summary>
        /// Get if the user informed the parameters.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [user got parameters]; otherwise, <c>false</c>.
        /// </value>
        bool UserGotParameters { get;  set; } 

        /// <summary>
        /// Gets or sets the default data source (SERVER / Instance). this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default data source.
        /// </value>
        string DefaultDataSource { get; set; }

        /// <summary>
        /// Gets or sets the default user. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default user.
        /// </value>
        string DefaultUser { get; set; } 

        /// <summary>
        /// Gets or sets the default password. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default password.
        /// </value>
        string DefaultPassword { get; set; } 

        /// <summary>
        /// Gets or sets the default database. this is used in the parameter forms by default value.
        /// </summary>
        /// <value>
        /// The default database.
        /// </value>
        string DefaultDatabase { get; set; } 

        /// <summary>
        /// Gets or sets a value that indicates whether the connection has been established correctly.
        /// </summary>
        /// <value>
        /// </value>
        bool IsConnectionSuccess { get; set; } 
        
        /// <summary>
        /// Determine if it should be written to the configuration file. It will be written only if the connection was successful.
        /// </summary>
        bool WriteInConfigFile { get; set; } 

        /// <summary>
        /// Determines whether the parameters must be encrypted / decrypted.
        /// </summary>
        bool IsEncryptedData { get; set; } 

        /// <summary>
        /// Try to get the connection string from the app configuration file. Need a ConnectionStringName
        /// <seealso cref="ConnectionStringName"/>
        /// </summary>
        bool TryGetConnectionStringFromConfig { get; set; } 
        
        /// <summary>
        /// Gets the last error message.
        /// </summary>
        string ConnectionError { get; set; } 

        /// <summary>
        /// Gets or sets the connector parameter type used.
        /// </summary>
        /// <value>
        /// The connector parameter type used.
        /// </value>
        EConnectorParameterTypeUsed ConnectorParameterTypeUsed { get; set; } 

        /// <summary>
        /// try to connect to the specified parameters.
        /// </summary>
        void Connect();

    }
}
