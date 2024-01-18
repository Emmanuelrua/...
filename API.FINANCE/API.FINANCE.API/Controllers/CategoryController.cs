using API.FINANCE.Data;
using API.FINANCE.Data.Migrations;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FINANCE.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly APIFinanceContext _context;

        public CategoryController(APIFinanceContext context, UserManager<IdentityUser> userManager)
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

            var Category = await _context.Categories.Where(a=> a.UserId == token.UserId).ToListAsync();

            return Ok(Category);

        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> Post(CategoryRequestPost category)
        {
            var token= await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);

            if (token == null) 
                return BadRequest(new AuthResultCategory(){
                Result = false,
                Errors = new List<string>() { "Token invalided" }
            });

            var newCategory = new Category()
            {
                Token = token.Token,
                UserId = token.UserId,
                NameCategory = category.NameCategory,
                Money = category.Money,
                DescriptionCategory = category.DescriptionCategory
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return Ok(new AuthResultCategory()
            {
                Result = true,
            });
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

            _context.Categories.Remove(_context.Categories.FirstOrDefault(b => b.NameCategory == category.NameCategory));
         
            await _context.SaveChangesAsync();

            return Ok(new AuthResultCategory()
            {
                Result = true,
            }); 

        }
    }
}
