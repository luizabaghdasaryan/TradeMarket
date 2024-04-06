using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("receipt_details")]
    public class ReceiptDetail : BaseEntity
    {
        [ForeignKey("Receipt")]
        [Column("receipt_id")]
        public int ReceiptId { get; set; }

        [ForeignKey("Product")]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("discount_unit_price")]
        public decimal DiscountUnitPrice { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public virtual Receipt Receipt { get; set; }
        public virtual Product Product { get; set; }

        public ReceiptDetail() : base() { } 
        public ReceiptDetail(int productId, int receiptId, int quantity)
        {
            ProductId = productId;
            ReceiptId = receiptId;
            Quantity = quantity;
        }
    }
}