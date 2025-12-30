using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.DTOs;



namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetAllBrands()
        {
            var brands = _context.BrandMst
                .Where(b => b.IsActive)
                .Select(b => new BrandDto
                {
                    Brand_Id = b.Brand_Id,
                    Brand_Name = b.Brand_Name
                })
                .ToList();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public IActionResult GetBrandById(int id)
        {
            var brand = _context.BrandMst
                .Where(b => b.Brand_Id == id && b.IsActive)
                .Select(b => new BrandDto
                {
                    Brand_Id = b.Brand_Id,
                    Brand_Name = b.Brand_Name
                })
                .FirstOrDefault();

            if (brand == null)
                return NotFound("Brand not found");

            return Ok(brand);
        }

        [HttpGet("category/{categoryId}")]
        
        public IActionResult GetBrandsByCategory (int categoryId)
        {
            var brands = (
                from cbm in _context.CategoryBrandMapping
                join b in _context.BrandMst on cbm.Brand_Id equals b.Brand_Id
                where cbm.Category_Id == categoryId && cbm.IsActive && b.IsActive
                select new BrandDto
                {
                    Brand_Id = b.Brand_Id,
                    Brand_Name = b.Brand_Name
                }
              ).ToList();
            return Ok(brands);
        }

        [HttpPost]

        public IActionResult SaveBrand(BrandDto dto)
        {
                BrandMst brand;

            if (dto.Brand_Id == 0)
            {
                brand = new BrandMst
                {
                    Brand_Name = dto.Brand_Name,
                    IsActive = true
                };

                _context.BrandMst.Add(brand);
            }
            else
            {
                brand = _context.BrandMst.FirstOrDefault(b => b.Brand_Id == dto.Brand_Id && b.IsActive);

                if (brand == null)
                    return NotFound("Brand Not Found");

                brand.Brand_Name = dto.Brand_Name;
            }

            _context.SaveChanges();
            return Ok("Brand Saved Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _context.BrandMst
                .FirstOrDefault(b => b.Brand_Id == id && b.IsActive);

            if (brand == null)
                return NotFound("Brand not found");

            brand.IsActive = false;
            _context.SaveChanges();

            return Ok("Brand deleted successfully");
        }
    }
}