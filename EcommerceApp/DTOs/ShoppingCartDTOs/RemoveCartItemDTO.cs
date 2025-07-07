﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.DTOs.ShoppingCartDTOs
{
    public class RemoveCartItemDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "CartItemId is required.")]
        public int CartItemId { get; set; }
    }
}
