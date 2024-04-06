using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("receipt")]
    public class Receipt : BaseEntity
    {
        [ForeignKey("Customer")]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("operation_time")]
        public DateTime OperationDate { get; set; }

        [Column("is_checked_out")]
        public bool IsCheckedOut { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }

        public Receipt() : base() { }
    }
}