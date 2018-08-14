using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.Common
{
    public class DataRecordDynamicWrapper : DynamicObject
    {
        private IDataRecord _dataRecord;
        public DataRecordDynamicWrapper(IDataRecord dataRecord)
        {
            _dataRecord = dataRecord;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dataRecord[binder.Name];
            return result != null;
        }
    }
}
