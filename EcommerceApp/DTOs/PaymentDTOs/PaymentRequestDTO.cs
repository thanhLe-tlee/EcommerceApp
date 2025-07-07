﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.PaymentDTOs
{
    public class PaymentRequestDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Payment Method is required.")]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // e.g., "CreditCard", "DebitCard", "PayPal", "COD"
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
