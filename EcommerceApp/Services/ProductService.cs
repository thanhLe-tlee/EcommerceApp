using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.ProductDTOs;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace EcommerceApp.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductCreateDTO productDto)
        {
            try
            {
                if (await _context.Products.AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Product name already exists.");
                }
                if (!await _context.Categories.AnyAsync(cat => cat.Id == productDto.CategoryId))
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Specified category does not exist.");
                }
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    ImageUrl = productDto.ImageUrl,
                    CategoryId = productDto.CategoryId,
                    DiscountPercent = productDto.DiscountPercent,
                    IsAvailable = true
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                var productResponse = new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable,
                    DiscountPercent = product.DiscountPercent
                };
                return new ApiResponse<ProductResponseDTO>(200, productResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ProductResponseDTO>> GetProductByNameAsync(string name)
        {
            try
            {
                var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
                if (product == null)
                {
                    return new ApiResponse<ProductResponseDTO>(404, "Product not found.");
                }
                var productResponse = new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable,
                    DiscountPercent = product.DiscountPercent
                };
                return new ApiResponse<ProductResponseDTO>(200, productResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductAsync(ProductUpdateDTO productDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(productDto.Id);
                if (product == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }
                if (await _context.Products.AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower() && p.Id != productDto.Id))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Another product with the same name already exists.");
                }
                if (!await _context.Categories.AnyAsync(cat => cat.Id == productDto.CategoryId))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Specified category does not exist.");
                }
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.StockQuantity = productDto.StockQuantity;
                product.ImageUrl = productDto.ImageUrl;
                product.CategoryId = productDto.CategoryId;
                product.DiscountPercent = productDto.DiscountPercent;
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Product with Id {productDto.Id} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }
                _context.Products .Remove(product);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Product with Id {id} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();
                var productList = products.Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    IsAvailable = p.IsAvailable,
                    DiscountPercent = p.DiscountPercent,
                }).ToList();
                return new ApiResponse<List<ProductResponseDTO>>(200, productList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsAvailable)
                .ToListAsync();
                if (products == null || products.Count == 0)
                {
                    return new ApiResponse<List<ProductResponseDTO>>(404, "Products not found.");
                }
                var productList = products.Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    IsAvailable = p.IsAvailable,
                    DiscountPercent = p.DiscountPercent,
                }).ToList();
                return new ApiResponse<List<ProductResponseDTO>>(200, productList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductStatusAsync(ProductStatusUpdateDTO productStatusUpdateDTO)
        {
            try
            {
                var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productStatusUpdateDTO.ProductId);
                if (product == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }
                product.IsAvailable = productStatusUpdateDTO.IsAvailable;
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Product with Id {productStatusUpdateDTO.ProductId} Status Updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<ProductResponseDTO>>> SearchForProductsAsync(string? categoryName, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category.Name.ToLower() == categoryName.ToLower());
            }

            if(minPrice != null)
            {
                query = query.Where(p => p.Price >=  minPrice);
            }

            if (maxPrice != null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            var result = await query.ToListAsync();

            var productList = result.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable,
                DiscountPercent = p.DiscountPercent,
            }).ToList();

            if (result.Count == 0) 
            {
                return new ApiResponse<List<ProductResponseDTO>>(404, "Product not found");
            }

            return new ApiResponse<List<ProductResponseDTO>>(200, productList);
            
        }
    }
}