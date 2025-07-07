using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.AddressesDTOs;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Services
{
    public class AddressService
    {
        private readonly ApplicationDbContext _context;
        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(addressDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Customer not found.");
                }

                var address = new Address
                {
                    CustomerId = addressDto.CustomerId,
                    Address1 = addressDto.Address1,
                    City = addressDto.City,
                    Country = addressDto.Country
                };
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                var addressResponse = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    Address1 = address.Address1,
                    City = address.City,
                    Country = address.Country
                };
                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
               return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(add => add.Id == id);
                if (address == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Address not found.");
                }
                var addressResponse = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    Address1 = address.Address1,
                    City = address.City,
                    Country = address.Country
                };
                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            try
            {
                var address = await _context.Addresses
                .FirstOrDefaultAsync(add => add.Id == addressDto.AddressId && add.CustomerId == addressDto.CustomerId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
                }
                address.Address1 = addressDto.Address1;
                address.City = addressDto.City;
                address.Country = addressDto.Country;
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO)
        {
            try
            {
                var address = await _context.Addresses
                .FirstOrDefaultAsync(add => add.Id == addressDeleteDTO.AddressId && add.CustomerId == addressDeleteDTO.CustomerId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
                }
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDeleteDTO.AddressId} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "An unexpected error occurred while processing your request.");
            }
        }
        public async Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    return new ApiResponse<List<AddressResponseDTO>>(404, "Customer not found.");
                }
                var addresses = customer.Addresses.Select(a => new AddressResponseDTO
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    Address1 = a.Address1,
                    City = a.City,
                    Country = a.Country
                }).ToList();
                return new ApiResponse<List<AddressResponseDTO>>(200, addresses);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AddressResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
