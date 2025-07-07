using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required(ErrorMessage = "At least one order item is required.")]
        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<OrderItemCreateDTO> OrderItems { get; set; }
    }
}
