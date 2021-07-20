using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Mapping;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericRepository;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var persons = new Persons(@"Data Source =NT-SYSWORK; Initial Catalog=TEST_SYSWORK_DATA; User ID=SA; Password=Dm58125812", EDatabaseEngine.MSSqlServer);

            var per = persons.GetByLambdaExpressionFilter(p => p.FirstName.Trim().ToLower().ToUpper() == "FName PWY85CBA62");
        }
    }

    public class Persons : BaseRepository<Person>
    {
        public Persons(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        {
        }
    }

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
        
        public string NotMapped { get; set; }
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
