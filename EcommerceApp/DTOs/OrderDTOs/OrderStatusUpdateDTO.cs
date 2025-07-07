using EcommerceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.OrderDTOs
{
    public class OrderStatusUpdateDTO
    {
        [Required(ErrorMessage = "OrderId is required.")]
        public int OrderId { get; set; }

        [Required]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid Order Status.")]
        public OrderStatus OrderStatus { get; set; }
    }
}
