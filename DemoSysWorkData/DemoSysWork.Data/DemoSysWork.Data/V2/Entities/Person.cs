using System;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Mapping;

namespace Demo.SysWork.Data.Entities
{
    [Table(Name = "Persons")]
    public class Person
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
        public DateTime? BirthDate { get; set; }
        [Column(Name = "Long Name Field")]
        public string LongNameField { get; set; }
        [Column()]
        public bool Active { get; set; }
        //[Column()]
        //public Guid GUID { get; set; }
        public Person()
        {

        }
        public Person(long idPerson, string firstName, string lastName, string passport, string address, long? idState, DateTime? birthDate, string longNameField, bool active)
        {
            IdPerson = idPerson;
            FirstName = firstName;
            LastName = lastName;
            Passport = passport;
            Address = address;
            IdState = idState;
            BirthDate = birthDate;
            LongNameField = longNameField;
            Active = active;
        }
    }
}



