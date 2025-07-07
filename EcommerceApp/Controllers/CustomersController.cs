using EcommerceApp.DTOs;
using EcommerceApp.DTOs.CustomerDTOs;
using EcommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("RegisterCustomer")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> RegisterCustomer([FromBody] CustomerRegistrationDTO customerDto)
        {
            var response = await _customerService.RegisterCustomerAsync(customerDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginDTO loginDto)
        {
            var response = await _customerService.LoginAsync(loginDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetCustomerById/{id}")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetCustomerById(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCustomer([FromBody] CustomerUpdateDTO customerDto)
        {
            var response = await _customerService.UpdateCustomerAsync(customerDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(int id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var response = await _customerService.ChangePasswordAsync(changePasswordDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomersAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllCustomerAccounts")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetAllCustomerAccounts()
        {
            var response = await _customerService.GetAllCustomerAccountsAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
