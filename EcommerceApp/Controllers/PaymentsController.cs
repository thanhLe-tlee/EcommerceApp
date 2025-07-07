using EcommerceApp.DTOs;
using EcommerceApp.DTOs.PaymentDTOs;
using EcommerceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        [HttpPost("ProcessPayment")]
        public async Task<ActionResult<ApiResponse<PaymentResponseDTO>>> ProcessPayment([FromBody] PaymentRequestDTO paymentRequest)
        {
            var response = await _paymentService.ProcessPaymentAsync(paymentRequest);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        
        [HttpGet("GetPaymentById/{paymentId}")]
        public async Task<ActionResult<ApiResponse<PaymentResponseDTO>>> GetPaymentById(int paymentId)
        {
            var response = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        
        [HttpGet("GetPaymentByOrderId/{orderId}")]
        public async Task<ActionResult<ApiResponse<PaymentResponseDTO>>> GetPaymentByOrderId(int orderId)
        {
            var response = await _paymentService.GetPaymentByOrderIdAsync(orderId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        
        [HttpPut("UpdatePaymentStatus")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdatePaymentStatus([FromBody] PaymentStatusUpdateDTO statusUpdate)
        {
            var response = await _paymentService.UpdatePaymentStatusAsync(statusUpdate);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        
        [HttpPost("CompleteCODPayment")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> CompleteCODPayment([FromBody] CODPaymentUpdateDTO codPaymentUpdateDTO)
        {
            var response = await _paymentService.CompleteCODPaymentAsync(codPaymentUpdateDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
