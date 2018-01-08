using System.Dynamic;
using System.Data;

namespace SysWork.Data.DbUtil
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
