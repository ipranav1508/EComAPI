using EComAPI.Data;
using EComAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComAPI.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE CATEGORY
        [HttpPost]
        [Route("api/categories/CreateCategory")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category created successfully",
                category
            });
        }

        // GET ALL CATEGORIES
        [HttpGet]
        [Route("api/categories/GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET CATEGORY BY ID
        [HttpGet]
        [Route("api/categories/GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // UPDATE CATEGORY
        [HttpPut]
        [Route("api/categories/UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = dto.Name;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category updated successfully"
            });
        }

        // DELETE CATEGORY
        [HttpDelete]
        [Route("api/categories/DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category deleted successfully"
            });
        }

        // ASSIGN CATEGORY TO PRODUCT
        [HttpPut]
        [Route("api/categories/AssignCategory/{productId}")]
        public async Task<IActionResult> AssignCategoryToProduct(
            int productId,
            [FromBody] int categoryId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            var category = await _context.Categories
                .FindAsync(categoryId);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            // IMPORTANT FIX
            product.CategoryId = categoryId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category assigned successfully"
            });
        }
    }
}