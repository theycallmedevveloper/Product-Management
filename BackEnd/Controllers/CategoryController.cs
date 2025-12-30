using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.DTOs;



namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetAllCategories()
        {
            var categories = _context.CategoryMst   
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    Category_Id = c.Category_Id,
                    Category_Name = c.Category_Name
                })
                .ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.CategoryMst
                .Where(c => c.Category_Id == id && c.IsActive)
                .Select(c => new CategoryDto
                {
                    Category_Id = c.Category_Id,
                    Category_Name = c.Category_Name
                })
                .FirstOrDefault();

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }


        [HttpGet("main")]
        public IActionResult GetMainCategories()
        {
            var data = _context.CategoryMst
                .Where(x => x.Parent_Category_Id == null && x.IsActive)
                .ToList();

            return Ok(data);
        }


        [HttpGet("sub/{parentId}")]
        public IActionResult GetSubCategories(int parentId)
        {
            var data = _context.CategoryMst
                .Where(x => x.Parent_Category_Id == parentId && x.IsActive)
                .ToList();

            return Ok(data);
        }

        [HttpPost]
        public IActionResult SaveCategory(CategoryDto dto)
        {
            CategoryMst category;

            if (dto.Category_Id == 0)
            {
                category = new CategoryMst
                {
                    Category_Name = dto.Category_Name,
                    IsActive = true
                };

                _context.CategoryMst.Add(category);
            }
            else
            {
                category = _context.CategoryMst.FirstOrDefault(c => c.Category_Id == dto.Category_Id && c.IsActive);

                if (category == null)
                    return NotFound("Category Not Found");

                category.Category_Name = dto.Category_Name;
            }

            _context.SaveChanges();
            return Ok("Category Saved Successfully"); // FIXED
        }
    }
}
