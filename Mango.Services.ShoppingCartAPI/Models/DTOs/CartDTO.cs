﻿using System.Collections.Generic;

namespace Mango.Services.ShoppingCartAPI.Models.DTOs
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeader { get; set; }
        public IEnumerable<CartDetailsDTO> CartDetails { get; set; }
    }
}
