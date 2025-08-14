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
        private readonly CookieCartService _cookieCartService;
        public CartsController(ShoppingCartService shoppingCartService, CookieCartService cookieCartService)
        {
            _shoppingCartService = shoppingCartService;
            _cookieCartService = cookieCartService;
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

        // New endpoints for cookie-based cart (anonymous users)
        [HttpGet("cookie")]
        public ActionResult<List<CookieCartDTO>> GetCookieCart()
        {
            return Ok(_cookieCartService.GetCart());
        }

        [HttpPost("cookie/add")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> AddToCookieCart(
            [FromQuery] int productId,
            [FromQuery] int quantity)
        {
            var response = await _cookieCartService.AddToCart(productId, quantity);
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("cookie/update")]
        public ActionResult<ApiResponse<ConfirmationResponseDTO>> UpdateCookieCartItem(
            [FromQuery] int productId,
            [FromQuery] int quantity)
        {
            var response = _cookieCartService.UpdateCartItemQuantity(productId, quantity);
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("cookie/remove/{productId}")]
        public ActionResult<ApiResponse<ConfirmationResponseDTO>> RemoveFromCookieCart(int productId)
        {
            var response = _cookieCartService.RemoveFromCart(productId);
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("cookie/clear")]
        public ActionResult<ApiResponse<ConfirmationResponseDTO>> ClearCookieCart()
        {
            var response = _cookieCartService.ClearCart();
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
