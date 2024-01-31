using API.FINANCE.Data;
using API.FINANCE.Data.Migrations;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.CategoryMethods.verifications;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.FINANCE.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly APIFinanceContext _context;

        public CategoryController(APIFinanceContext context)
        {
            _context = context;
        }
        [HttpGet("{Token}")]
        public async Task<IActionResult> Get(string Token)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == Token);

            if (token == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                });

            var Category = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();

            return Ok(Category);

        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> Post(CategoryRequestPost category)
        {     
            
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);
            MySalary? ExistSalary = null;

            if (token != null)
            {
                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);
            }

            AuthResultCategory result = await CategoryVerificationsPost.verificationPost(category,token, ExistSalary);

            if (result.Result == true)
            {
                var newCategory = new Category()
                {
                    Token = token.Token,
                    UserId = token.UserId,
                    NameCategory = category.NameCategory,
                    Money = category.Money,
                    DescriptionCategory = category.DescriptionCategory,
                    AddedDate = DateTime.UtcNow,
                    IsExpired = DateTime.UtcNow.AddDays(30)
                };

                ExistSalary.Salary = ExistSalary.Salary - category.Money;
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(CategoryRequestDelete category)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);

            if (token == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                });

            var Category = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();

            var ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);

            if (category == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have a category" }
                });

            var CategoryDelete = _context.Categories.FirstOrDefault(b => b.NameCategory == category.NameCategory);

            if (CategoryDelete == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Name invalided" }
                });


            ExistSalary.Salary = ExistSalary.Salary + CategoryDelete.Money;

            _context.Categories.Remove(CategoryDelete);
            await _context.SaveChangesAsync();

            return Ok(new AuthResultCategory()
            {
                Result = true,
            });

        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Put(CategoryRequestPost category)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);

            List<Category>? CategoryList = null;     MySalary? ExistSalary = null;        Category? CategoryUpdate = null;

            if (token != null)
            {
                CategoryList = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();

                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);

                CategoryUpdate = _context.Categories.FirstOrDefault(b => b.NameCategory == category.NameCategory);
            }

            AuthResultCategory result = await CategoryVerificationsPut.verificationPut(token, category, ExistSalary, CategoryUpdate);

            if (result.Result == true)
            {
                var Totalcategories = CategoryUpdate.Money - category.Money;

                ExistSalary.Salary = ExistSalary.Salary + Totalcategories;

                CategoryUpdate.Money = category.Money;

                CategoryUpdate.DescriptionCategory = category.DescriptionCategory;

                await _context.SaveChangesAsync();

                return Ok(new AuthResultCategory()
                {
                    Result = true,
                });
            }
            return BadRequest(result);
        }
    }
}