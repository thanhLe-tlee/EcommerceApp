﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.ProductDTOs
{
    public class ProductStatusUpdateDTO
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "IsAvailable is required.")]
        public bool IsAvailable { get; set; }

        public bool IsFeatured { get; set; }
    }
}
