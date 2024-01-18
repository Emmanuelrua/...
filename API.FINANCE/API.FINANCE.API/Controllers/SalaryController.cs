using API.FINANCE.Data;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.Common;
using API.FINANCE.Shared.DTOs.MySalaryRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FINANCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly APIFinanceContext _context;

        public SalaryController(APIFinanceContext context, UserManager<IdentityUser> userManager)
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

            var Salary = await _context.Salaries.Where(a => a.UserId == token.UserId).ToListAsync();

            return Ok(Salary);

        }

        [HttpPost("AddSalary")]
        public async Task<IActionResult> Post(MySalaryRequest request)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == request.Token);

            if (token == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                });

            var ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);

            if (ExistSalary != null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you already have a salary" }
                });

            if (request.Salary <= 0)
            {
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Salary invalided" }
                });
            }

            var newSalary = new MySalary()
            {
                SalaryId = RandomGenerator.GenerateRandomString(10),
                UserId=token.UserId,
                Salary=request.Salary,
                Token=token.Token,
                Message=request.Message,
            };

            await _context.Salaries.AddAsync(newSalary);
            await _context.SaveChangesAsync();

            return Ok(new AuthResultCategory()
            {
                Result = true
            });
        }
        [HttpPut("UpdateSalary")]
        public async Task<IActionResult> Put(MySalaryRequest request)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == request.Token);

            if (token == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                });

            var ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);

            if (ExistSalary == null)
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have a salary created" }
                });

            if (request.Salary <= 0)
            {
                return BadRequest(new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Salary invalided" }
                });
            }

            ExistSalary.Salary = request.Salary;
            ExistSalary.Message = request.Message;

            await _context.SaveChangesAsync();

            return Ok(new AuthResultCategory()
            {
                Result = true
            });
        }
    }
}
