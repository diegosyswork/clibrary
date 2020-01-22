using System;
using SysWork.Data.GenericRepository.Attributes;

namespace Demo.SysWork.Data.Entities
{
    [DbTable(Name = "Persons")]
    public class Person
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
        public DateTime? BirthDate { get; set; }
        [DbColumn(ColumnName = "Long Name Field")]
        public string LongNameField { get; set; }
        [DbColumn()]
        public bool Active { get; set; }
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



