using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.DTOs;
using ProductManagement.Models;
using System.Globalization;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = (
                from p in _context.ProductMst
                join sub in _context.CategoryMst on p.Category_Id equals sub.Category_Id
                join main in _context.CategoryMst on sub.Parent_Category_Id equals main.Category_Id
                join b in _context.BrandMst on p.Brand_Id equals b.Brand_Id
                where p.IsActive && sub.IsActive && main.IsActive && b.IsActive
                select new
                {
                    Product_Id = p.Product_Id,
                    Product_Name = p.Product_Name,
                    Product_Code = p.Product_Code,
                    Price = p.Price,
                    Mfg_Date = p.Mfg_Date.ToString("dd/MM/yyyy"),    
                    Expiry_Date = p.Expiry_Date.HasValue ? p.Expiry_Date.Value.ToString("dd/MM/yyyy") : null,

                    MainCategory = new
                    {
                        Category_Id = main.Category_Id,
                        Category_Name = main.Category_Name
                    },

                    SubCategory = new
                    {
                        Category_Id = sub.Category_Id,
                        Category_Name = sub.Category_Name
                    },

                    Brand = new
                    {
                        Brand_Id = b.Brand_Id,
                        Brand_Name = b.Brand_Name
                    }
                }
            ).ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = (
                from p in _context.ProductMst
                join sub in _context.CategoryMst on p.Category_Id equals sub.Category_Id
                join main in _context.CategoryMst on sub.Parent_Category_Id equals main.Category_Id into mainGroup
                from main in mainGroup.DefaultIfEmpty()
                join b in _context.BrandMst on p.Brand_Id equals b.Brand_Id
                where p.Product_Id == id && p.IsActive
                select new
                {
                    Product_Id = p.Product_Id,
                    Product_Name = p.Product_Name,
                    Product_Code = p.Product_Code,
                    Price = p.Price,
                    Mfg_Date = p.Mfg_Date.ToString("dd/MM/yyyy"),
                    Expiry_Date = p.Expiry_Date.HasValue ? p.Expiry_Date.Value.ToString("dd/MM/yyyy") : null,

                    SubCategory = new
                    {
                        Category_Id = sub.Category_Id,
                        Category_Name = sub.Category_Name
                    },

                    MainCategory = main != null ? new
                    {
                        Category_Id = main.Category_Id,
                        Category_Name = main.Category_Name
                    } : null,

                    Brand = new
                    {
                        Brand_Id = b.Brand_Id,
                        Brand_Name = b.Brand_Name
                    }
                }
            ).FirstOrDefault();

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductRequestDto dto)
        {
            try
            {
                ProductMst product;
                var now = DateTime.Now;

                if (dto.Product_Id == 0)
                {
                    product = new ProductMst
                    {
                        Entry_Date = now
                    };
                    _context.ProductMst.Add(product);
                }
                else
                {
                    product = await _context.ProductMst.FirstOrDefaultAsync(p => p.Product_Id == dto.Product_Id && p.IsActive);

                    if (product == null)
                        return NotFound("Product not found");
                }

                product.Product_Name = dto.Product_Name;
                product.Product_Code = dto.Product_Code;
                product.Category_Id = dto.Category_Id;
                product.Brand_Id = dto.Brand_Id;
                product.Price = dto.Price;

                product.Mfg_Date = DateOnly.ParseExact(
                    dto.Mfg_Date,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture
                );

                product.Expiry_Date = string.IsNullOrEmpty(dto.Expiry_Date) ? null: DateOnly.ParseExact(
                        dto.Expiry_Date,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture
                    );

                product.Update_Date = now;
                product.IsActive = true;

                if (dto.Price <= 0)
                    return BadRequest("Price must be greater than 0");

                if (product.Expiry_Date.HasValue && product.Expiry_Date.Value <= product.Mfg_Date)
                { 
                    return BadRequest("Expiry date must be after manufacturing date");
                }

                await _context.SaveChangesAsync();

                return Ok("Product saved successfully");
            }

            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.ProductMst
                .FirstOrDefault(p => p.Product_Id == id && p.IsActive);

            if (product == null)
                return NotFound("Product not found");

            product.IsActive = false;
            _context.SaveChanges();

            return Ok("Product deleted successfully");
        }

    }
}
