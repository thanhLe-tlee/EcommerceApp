using EcommerceApp.DTOs;
using EcommerceApp.DTOs.ShoppingCartDTOs;
using EcommerceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ShoppingCartService _shoppingCartService;
        public CartsController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("GetCart/{customerId}")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> GetCartByCustomerId(int customerId)
        {
            var response = await _shoppingCartService.GetCartByCustomerIdAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> AddToCart([FromBody] AddToCartDTO addToCartDTO)
        {
            var response = await _shoppingCartService.AddToCartAsync(addToCartDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateCartItem")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> UpdateCartItem([FromBody] UpdateCartItemDTO updateCartItemDTO)
        {
            var response = await _shoppingCartService.UpdateCartItemAsync(updateCartItemDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("RemoveCartItem")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> RemoveCartItem([FromBody] RemoveCartItemDTO removeCartItemDTO)
        {
            var response = await _shoppingCartService.RemoveCartItemAsync(removeCartItemDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("ClearCart")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ClearCart([FromQuery] int customerId)
        {
            var response = await _shoppingCartService.ClearCartAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
