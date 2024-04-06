using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("person")]
    public class Person : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        public Customer Customer { get; set; }

        public Person() : base() { }

        public Person(int id, string name, string surname, DateTime birthDate) : base(id)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
        }
    }
}
