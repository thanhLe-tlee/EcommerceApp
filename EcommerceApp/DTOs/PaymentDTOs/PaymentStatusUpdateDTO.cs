﻿using EcommerceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.PaymentDTOs
{
    public class PaymentStatusUpdateDTO
    {
        [Required(ErrorMessage = "Payment ID is required.")]
        public int PaymentId { get; set; }
        public string? TransactionId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public PaymentStatus Status { get; set; } // e.g., "Completed", "Failed"
    }
}
