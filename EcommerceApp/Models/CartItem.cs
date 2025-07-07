using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required] 
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required, Range(1, 100)]
        public int Quantity { get; set; }

        [Required, Column(TypeName = "decimal(18,2)"), Range(0.00,double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)"), Range(0.0, double.MaxValue)]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
