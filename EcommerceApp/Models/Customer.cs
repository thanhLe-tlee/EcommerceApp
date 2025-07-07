using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    [Index(nameof(Email), Name = "IX_Email_Unique", IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstName is required."), StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName length must be between 2 and 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "FirstName is required."), StringLength(50, MinimumLength = 2, ErrorMessage = "LastName length must be between 2 and 50 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        
    }
}
