using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common.ObjectResolver
{
    public abstract class DataObjectCreator
    {
        public abstract IDbConnection GetIDbConnection();
        public abstract IDbConnection GetIDbConnection(string connectionString);
        public abstract DbConnection GetDbConnection();
        public abstract DbConnection GetDbConnection(string connectionString);
        public abstract IDbDataParameter GetIDbDataParameter();
        public abstract DbConnectionStringBuilder GetDbConnectionStringBuilder();
    }
}
