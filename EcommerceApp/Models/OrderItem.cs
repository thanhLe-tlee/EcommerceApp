using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        // Foreign key to Order
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        // Foreign key to Product
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
        [Range(0.00, double.MaxValue), Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Range(0.00, double.MaxValue), Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        [Range(0.00, double.MaxValue), Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
