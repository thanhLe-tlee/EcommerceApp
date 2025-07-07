using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        // Foreign key to Customer
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        // Foreign key to Product
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required, Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
  
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
