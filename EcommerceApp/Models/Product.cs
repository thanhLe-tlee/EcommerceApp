using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required."), StringLength(100, MinimumLength = 3, ErrorMessage = "Product Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required."), MinLength(10)]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue), Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Stock Quantity must be between 0 and 10000.")]
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        [Range(0, 100)]
        public int DiscountPercent { get; set; }
        public bool IsAvailable { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }

    }
}
