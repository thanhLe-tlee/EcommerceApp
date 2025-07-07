using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.CategoryDTOs
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Category Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
