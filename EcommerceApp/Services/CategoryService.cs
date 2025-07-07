using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.CategoryDTOs;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO categoryDto)
        {
            try
            {
                if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
                {
                    return new ApiResponse<CategoryResponseDTO>(400, "Category name already exists.");
                }
                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                var categoryResponse = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                };
                return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<CategoryResponseDTO>> GetCategoryByNameAsync(string name)
        {
            try
            {
                var category = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
                if (category == null)
                {
                    return new ApiResponse<CategoryResponseDTO>(404, "Category not found.");
                }
                var categoryResponse = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                };
                return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryDto.Id);
                if (category == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");
                }
                if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower() && c.Id != categoryDto.Id))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Another category with the same name already exists.");
                }
                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description;
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Category with Id {categoryDto.Id} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(string name)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
                if (category == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Category with Name {name} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .AsNoTracking()
                    .ToListAsync();
                var categoryList = categories.Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                }).ToList();
                return new ApiResponse<List<CategoryResponseDTO>>(200, categoryList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoryResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    } 
}