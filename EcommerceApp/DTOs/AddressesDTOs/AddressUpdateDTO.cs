using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.AddressesDTOs
{
    public class AddressUpdateDTO
    {
        [Required(ErrorMessage = "AddressId is required.")]
        public int AddressId { get; set; }
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Address Line 1 is required.")]
        [StringLength(100, ErrorMessage = "Address Line 1 cannot exceed 100 characters.")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string Country { get; set; }
    }
}
