using System;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Mapping;

namespace Demo.SysWork.Data.V2.Entities
{
    [View(Name = "VW_PersonsWithStates")]
    public class VW_PersonsWithStates
    {
        [Column(IsIdentity = true, IsPrimaryKey = true)]
        public long IdPerson { get; set; }
        [Column()]
        public string FirstName { get; set; }
        [Column()]
        public string LastName { get; set; }
        [Column()]
        public string Passport { get; set; }
        [Column()]
        public string Address { get; set; }
        [Column()]
        public long? IdState { get; set; }
        [Column()]
        public string StateCode { get; set; }
        [Column()]
        public string Description { get; set; }
        [Column()]
        public DateTime? BirthDate { get; set; }
        [Column()]
        public bool Active { get; set; }
        [Column(Name = "Long Name Field")]
        public string LongNameField { get; set; }

    }
}
