using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string OrderNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Customer Id is required.")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required, Column(TypeName = "decimal(18,2)"), Range(0.00, double.MaxValue)]
        public decimal TotalBaseAmount { get; set; }

        [Required, Column(TypeName = "decimal(18,2)"), Range(0.00, double.MaxValue)]
        public decimal TotalDiscountAmount { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address1 { get; set; }

        [Column(TypeName = "decimal(18,2)"), Range(0.00, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required, EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status.")]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment? Payment { get; set; }
    }
}
