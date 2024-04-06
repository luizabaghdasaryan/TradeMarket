using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("product_category")]
    public class ProductCategory : BaseEntity
    {
        [Column("category_name")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public ProductCategory() : base() { }
    }
}