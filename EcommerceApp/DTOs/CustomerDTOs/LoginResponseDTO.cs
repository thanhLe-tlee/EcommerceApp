﻿using Microsoft.Identity.Client;

namespace EcommerceApp.DTOs.CustomerDTOs
{
    public class LoginResponseDTO
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Message { get; set; }
    }
}
