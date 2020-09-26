using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SysWork.Data.Common.LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the supported aggregate SQL functions. The item names should match the related function names
    /// </summary>
    public enum SelectFunction
    {
        /// <summary>
        /// The count
        /// </summary>
        COUNT,

        /// <summary>
        /// The distinct
        /// </summary>
        DISTINCT,

        /// <summary>
        /// The sum
        /// </summary>
        SUM,

        /// <summary>
        /// The minimum
        /// </summary>
        MIN,

        /// <summary>
        /// The maximum
        /// </summary>
        MAX,

        /// <summary>
        /// The average
        /// </summary>
        AVG
    }
}
