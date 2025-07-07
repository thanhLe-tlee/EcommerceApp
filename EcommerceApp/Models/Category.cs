using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    [Index(nameof(Name), Name = "IX_Name_Unique", IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required."), StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
