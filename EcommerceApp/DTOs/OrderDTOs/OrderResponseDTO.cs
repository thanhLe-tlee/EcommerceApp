using EcommerceApp.Models;

namespace EcommerceApp.DTOs.OrderDTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalBaseAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; }
    }
}
