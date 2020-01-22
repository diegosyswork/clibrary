using System;
using SysWork.Data.GenericRepository.Attributes;

namespace Demo.SysWork.Data.V2.Entities
{
    [DbView(Name = "VW_PersonsWithStates")]
    public class VW_PersonsWithStates
    {
        [DbColumn(IsIdentity = true, IsPrimary = true)]
        public long IdPerson { get; set; }
        [DbColumn()]
        public string FirstName { get; set; }
        [DbColumn()]
        public string LastName { get; set; }
        [DbColumn()]
        public string Passport { get; set; }
        [DbColumn()]
        public string Address { get; set; }
        [DbColumn()]
        public long? IdState { get; set; }
        [DbColumn()]
        public string StateCode { get; set; }
        [DbColumn()]
        public string Description { get; set; }
        [DbColumn()]
        public DateTime? BirthDate { get; set; }
        [DbColumn()]
        public bool Active { get; set; }
        [DbColumn(ColumnName = "Long Name Field")]
        public string LongNameField { get; set; }

    }
}
