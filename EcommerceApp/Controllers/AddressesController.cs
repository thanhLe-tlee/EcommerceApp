using EcommerceApp.DTOs;
using EcommerceApp.DTOs.AddressesDTOs;
using EcommerceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AddressService _addressService;
        public AddressesController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("CreateAddress")]
        public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> CreateAddress([FromBody] AddressCreateDTO addressDto)
        {
            var response = await _addressService.CreateAddressAsync(addressDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetAddressById/{id}")]
        public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> GetAddressById(int id)
        {
            var response = await _addressService.GetAddressByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateAddress([FromBody] AddressUpdateDTO addressDto)
        {
            var response = await _addressService.UpdateAddressAsync(addressDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteAddress([FromBody] AddressDeleteDTO addressDeleteDTO)
        {
            var response = await _addressService.DeleteAddressAsync(addressDeleteDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetAddressesByCustomer/{customerId}")]
        public async Task<ActionResult<ApiResponse<List<AddressResponseDTO>>>> GetAddressesByCustomer(int customerId)
        {
            var response = await _addressService.GetAddressesByCustomerAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
