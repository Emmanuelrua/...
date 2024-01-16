using API.FINANCE.Data;
using API.FINANCE.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FINANCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategotyController : ControllerBase
    {
        private readonly APIFinanceContext _context;
        public CategotyController(APIFinanceContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Category>> Get() 
        {
            return await _context.Categories.ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            if (category == null) NotFound();

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Post", category.CategoryId, category);
        }
    }
}
