using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("customer")]
    public class Customer : BaseEntity
    {
        [ForeignKey("Person")]
        [Column("person_id")]
        public int PersonId { get; set; }

        [Column("discount")]
        public int DiscountValue { get; set; }

        public virtual Person Person { get; set; }
        public ICollection<Receipt> Receipts { get; set; }

        public Customer() : base() { }

        public Customer(int id, int personId, int discount) : base(id)
        {
            PersonId = personId;
            DiscountValue = discount;
        }
    }
}
