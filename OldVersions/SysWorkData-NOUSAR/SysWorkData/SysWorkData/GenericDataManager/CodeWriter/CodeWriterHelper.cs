using System;

namespace SysWork.Data.GenericDataManager.CodeWriter
{
    /// <summary>
    /// Helper to write code
    /// </summary>
    public static class DataManagerCodeWriterHelper
    {
        /// <summary>
        /// Adds the using.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string AddUsing(string assembly)
        {
            return string.Format("using {0};", assembly);
        }

        /// <summary>
        /// Starts the namespace.
        /// </summary>
        /// <param name="NameSpace">The name space.</param>
        /// <returns></returns>
        public static string StartNamespace(string NameSpace)
        {
            string ret;
            ret = string.Format("namespace {0}", NameSpace) + Environment.NewLine;
            ret += "{";
            return ret;
        }

        /// <summary>
        /// Ends the namespace.
        /// </summary>
        /// <returns></returns>
        public static string EndNamespace()
        {
            return "}" ;
        }

        /// <summary>
        /// Starts the class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="inherits">The inherits.</param>
        /// <returns></returns>
        public static string StartClass(string className, string inherits = null)
        {

            string ret = "";
            if (inherits==null) 
                ret = string.Format("\tpublic class {0}", className) + Environment.NewLine;
            else
                ret = string.Format("\tpublic class {0} : {1}", className, inherits) + Environment.NewLine;

            ret += "\t{";

            return ret;
        }

        /// <summary>
        /// Ends the class.
        /// </summary>
        /// <returns></returns>
        public static string EndClass()
        {
            return "\t}" ;
        }
    }
}
