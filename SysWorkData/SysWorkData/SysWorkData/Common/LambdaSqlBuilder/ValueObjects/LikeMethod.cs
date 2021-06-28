namespace SysWork.Data.Common.LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the supported string methods for the SQL LIKE statement. The item names should match the related string methods.
    /// </summary>
    public enum LikeMethod
    {
        /// <summary>
        /// The starts with
        /// </summary>
        StartsWith,

        /// <summary>
        /// The ends with
        /// </summary>
        EndsWith,

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        Contains,

        /// <summary>
        /// The equals
        /// </summary>
        Equals
    }

    public enum TrimMethod
    {
        /// <summary>
        /// The starts with
        /// </summary>
        Trim,

        /// <summary>
        /// The ends with
        /// </summary>
        TrimStart,

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        TrimEnd
    }
}
