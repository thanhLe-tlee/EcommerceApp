using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.CategoryDTOs;
using EcommerceApp.DTOs.CustomerDTOs;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EcommerceApp.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<CustomerResponseDTO>> RegisterCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            try
            {
                if (await _context.Customers.AnyAsync(c => c.Email.ToLower() == customerDto.Email.ToLower()))
                {
                    return new ApiResponse<CustomerResponseDTO>(400, "Email is already in used.");
                }

                var customer = new Models.Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                    DateOfBirth = customerDto.DateOfBirth,
                    //Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password)
                    Password = customerDto.Password
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                var customerResponse = new CustomerResponseDTO
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth
                };

                return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email);
                if (customer == null)
                {
                    return new ApiResponse<LoginResponseDTO>(401, "Invalid Email or password.");
                }
                // Verify password using BCrypt
                //bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password);
                bool isPasswordValid = loginDto.Password == customer.Password;
                if (!isPasswordValid)
                {
                    return new ApiResponse<LoginResponseDTO>(401, "Invalid email or password.");
                }
                var loginResponse = new LoginResponseDTO
                {
                    Message = "Login successful.",
                    CustomerId = customer.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}"
                };
                return new ApiResponse<LoginResponseDTO>(200, loginResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return new ApiResponse<CustomerResponseDTO>(404, "Customer not found.");
                }
                var customerResponse = new CustomerResponseDTO
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth
                };
                return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpdateDTO customerDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
                }
                if (customer.Email != customerDto.Email && await _context.Customers.AnyAsync(c => c.Email == customerDto.Email))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Email is already in use.");
                }
                
                customer.FirstName = customerDto.FirstName;
                customer.LastName = customerDto.LastName;
                customer.Email = customerDto.Email;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.DateOfBirth = customerDto.DateOfBirth;
                await _context.SaveChangesAsync();
   
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Customer with Id {customerDto.CustomerId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
                }
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Customer with Id {id} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(changePasswordDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found or inactive.");
                }
                // Verify current password
                //bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, customer.Password);
                bool isCurrentPasswordValid = changePasswordDto.CurrentPassword == customer.Password;
                if (!isCurrentPasswordValid)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(401, "Current password is incorrect.");
                }
                // Hash the new password
                //customer.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                customer.Password = changePasswordDto.NewPassword;
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = "Password changed successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CustomerResponseDTO>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _context.Customers.AsNoTracking().ToListAsync();
                var customerList = customers.Select(c => new CustomerResponseDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    DateOfBirth = c.DateOfBirth,
                }).ToList();

                return new ApiResponse<List<CustomerResponseDTO>>(200, customerList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CustomerResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CustomerResponseAccountDTO>>> GetAllCustomerAccountsAsync()
        {
            try
            {
                var customers = await _context.Customers.AsNoTracking().ToListAsync();
                var customerList = customers.Select(c => new CustomerResponseAccountDTO
                {
                    Id = c.Id,
                    Email = c.Email,
                    Password = c.Password,
                }).ToList();

                return new ApiResponse<List<CustomerResponseAccountDTO>>(200, customerList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CustomerResponseAccountDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
