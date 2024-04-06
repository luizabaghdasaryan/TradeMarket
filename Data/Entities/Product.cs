using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("product")]
    public class Product : BaseEntity
    {
        [ForeignKey("Category")]
        [Column("product_category_id")]
        public int ProductCategoryId { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
       
        public Product() : base() { }
    }
}