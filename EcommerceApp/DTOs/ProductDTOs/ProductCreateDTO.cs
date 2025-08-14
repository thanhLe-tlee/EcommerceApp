using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [MinLength(10)]
        public string Description { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, 10000, ErrorMessage = "Stock Quantity must be between 0 and 10000.")]
        public int StockQuantity { get; set; }
        [Url(ErrorMessage = "Invalid Image URL.")]
        public string ImageUrl { get; set; }
        [Range(0, 100, ErrorMessage = "Discount Percentage must be between 0% and 100%.")]
        public int DiscountPercent{ get; set; }
        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }

        public bool IsFeatured { get; set; }
    }
}
