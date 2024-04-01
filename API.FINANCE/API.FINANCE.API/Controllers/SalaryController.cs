using API.FINANCE.Data;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.Common;
using API.FINANCE.Shared.DTOs.MySalaryRequest;
using API.FINANCE.Shared.Methods.PercentageMethods.Operation;
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

        public SalaryController(APIFinanceContext context)
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
                AddedDate = DateTime.UtcNow,
                IsExpired = DateTime.UtcNow.AddDays(30),
                SalaryIn=request.Salary,
                Percentage =100
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

            var CategoryList = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();
            var IncomesList = await _context.Incomes.Where(i => i.UserId == token.UserId).ToListAsync();

            int WorthList = 0; double TotalPercentage = 0;
            int IncomesListT = 0; double TotalPercentageIncomes = 0;

            foreach (var worth in CategoryList)
            {
                var Percentage = await OperationPutSalary.operation(request, worth.Money);
                worth.Percentage = Percentage;
                await _context.SaveChangesAsync();
            }
            foreach (var worth in CategoryList)
            {
                TotalPercentage += worth.Percentage;
                WorthList += worth.Money;
            }
            foreach (var income in IncomesList)
            {
                var Percentage = await OperationPutSalary.operation(request, income.Money);
                income.Percentage = Percentage;
                await _context.SaveChangesAsync();
            }
            foreach (var income in IncomesList)
            {
                TotalPercentageIncomes += income.Percentage;
                IncomesListT += income.Money;
            }

            ExistSalary.Percentage = (100 - TotalPercentage) + TotalPercentageIncomes;
            ExistSalary.Salary = (request.Salary - WorthList) + IncomesListT;
            ExistSalary.SalaryIn = request.Salary;
            ExistSalary.Message = request.Message;

            await _context.SaveChangesAsync();

            if (ExistSalary.Salary < 0)
            {
                return Ok(new AuthResultCategory()
                {
                    Result = true,
                    Errors = new List<string>() { "It is recommended to lower expenses, since I am having losses" }
                });
            }

            return Ok(new AuthResultCategory()
            {
                Result = true
            });
        }
    }
}
