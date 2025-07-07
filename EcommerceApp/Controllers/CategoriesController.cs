using EcommerceApp.DTOs;
using EcommerceApp.DTOs.CategoryDTOs;
using EcommerceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> CreateCategory([FromBody] CategoryCreateDTO categoryDto)
        {
            var response = await _categoryService.CreateCategoryAsync(categoryDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetCategoryByName/{name}")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> GetCategoryById(string name)
        {
            var response = await _categoryService.GetCategoryByNameAsync(name);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCategory([FromBody] CategoryUpdateDTO categoryDto)
        {
            var response = await _categoryService.UpdateCategoryAsync(categoryDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteCategory/{name}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCategory(string name)
        {
            var response = await _categoryService.DeleteCategoryAsync(name);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ApiResponse<List<CategoryResponseDTO>>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
