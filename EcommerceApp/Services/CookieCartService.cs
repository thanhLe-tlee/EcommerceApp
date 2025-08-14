using EcommerceApp.DTOs;
using EcommerceApp.DTOs.ShoppingCartDTOs;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EcommerceApp.Services
{
    public class CookieCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProductService _productService;
        private const string CartCookieName = "ShoppingCart"; 

        public CookieCartService(IHttpContextAccessor httpContextAccessor, ProductService productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public ApiResponse<List<CookieCartDTO>> GetCart()
        {
            try
            {
                var cartCookie = _httpContextAccessor.HttpContext?.Request.Cookies[CartCookieName];
                var cart = new List<CookieCartDTO>();

                if (!string.IsNullOrEmpty(cartCookie))
                {
                    try
                    {
                        cart = JsonSerializer.Deserialize<List<CookieCartDTO>>(cartCookie);
                    }
                    catch
                    {
                        // If deserialization fails, return empty cart
                        cart = new List<CookieCartDTO>();
                    }
                }

                return new ApiResponse<List<CookieCartDTO>>(200, cart ?? new List<CookieCartDTO>());
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CookieCartDTO>>(500, $"Error retrieving cart: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> AddToCart(int productId, int quantity)
        {
            try
            {
                var cartResponse = GetCart();
                if (!cartResponse.Success)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(cartResponse.StatusCode, "Failed to retrieve cart");
                }

                var cart = cartResponse.Data;

                // Validate product exists and is available
                var productResult = await _productService.GetProductByNameAsync(productId.ToString());
                if (!productResult.Success)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found");
                }

                var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.Add(new CookieCartDTO
                    {
                        ProductId = productId,
                        Quantity = quantity
                    });
                }

                SaveCart(cart);
                return new ApiResponse<ConfirmationResponseDTO>(200,
                    new ConfirmationResponseDTO { Message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"Error adding item to cart: {ex.Message}");
            }
        }

        public ApiResponse<ConfirmationResponseDTO> UpdateCartItemQuantity(int productId, int quantity)
        {
            try
            {
                var cartResponse = GetCart();
                if (!cartResponse.Success)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(cartResponse.StatusCode, "Failed to retrieve cart");
                }

                var cart = cartResponse.Data;
                var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);

                if (existingItem == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Item not found in cart");
                }

                if (quantity <= 0)
                {
                    cart.Remove(existingItem);
                }
                else
                {
                    existingItem.Quantity = quantity;
                }

                SaveCart(cart);
                return new ApiResponse<ConfirmationResponseDTO>(200,
                    new ConfirmationResponseDTO { Message = "Cart updated successfully" });
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"Error updating cart: {ex.Message}");
            }
        }

        public ApiResponse<ConfirmationResponseDTO> RemoveFromCart(int productId)
        {
            try
            {
                var cartResponse = GetCart();
                if (!cartResponse.Success)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(cartResponse.StatusCode, "Failed to retrieve cart");
                }

                var cart = cartResponse.Data;
                var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);

                if (existingItem == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Item not found in cart");
                }

                cart.Remove(existingItem);
                SaveCart(cart);

                return new ApiResponse<ConfirmationResponseDTO>(200,
                    new ConfirmationResponseDTO { Message = "Item removed from cart successfully" });
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"Error removing item from cart: {ex.Message}");
            }
        }

        public ApiResponse<ConfirmationResponseDTO> ClearCart()
        {
            try
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete(CartCookieName);
                    return new ApiResponse<ConfirmationResponseDTO>(200,
                        new ConfirmationResponseDTO { Message = "Cart cleared successfully" });
                }
                return new ApiResponse<ConfirmationResponseDTO>(500, "Could not access HTTP context");
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"Error clearing cart: {ex.Message}");
            }
        }

        private void SaveCart(List<CookieCartDTO> cart)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax  // Changed from Strict to Lax for better browser compatibility
            };

            var cartJson = JsonSerializer.Serialize(cart);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(CartCookieName, cartJson, cookieOptions);
        }
    }
}