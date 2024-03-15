using API.FINANCE.Data;
using API.FINANCE.Data.Migrations;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using API.FINANCE.Shared.DTOs.IncomeRequest;
using API.FINANCE.Shared.Methods.IncomeMethods.verifications;
using API.FINANCE.Shared.Methods.PercentageMethods.Category;
using API.FINANCE.Shared.Methods.PercentageMethods.Operation;
using API.FINANCE.Shared.Methods.PercentageMethods.Return;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FINANCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly APIFinanceContext _context;

        public IncomeController(APIFinanceContext context)
        {
            _context = context;
        }

        [HttpGet("{Token}")]
        public async Task<IActionResult> Get(string Token)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == Token);
            List<Income>? incomes = null;

            if (token != null)
                incomes = await _context.Incomes.Where(a => a.UserId == token.UserId).ToListAsync();

            var result = await IncomeVerificationsGet.verificationGet(token);

            if(result.Result)
                return Ok(incomes);

            return BadRequest(result);

        }

        [HttpPost("AddIncome")]
        public async Task<IActionResult> Post(IncomeRequestPost income)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == income.Token);

            MySalary? ExistSalary = null; List<Income>? ListIncome = null; bool IncomeExists = false; 

            if (token != null)
            {
                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(i => i.UserId == token.UserId);
                ListIncome = await _context.Incomes.Where(c => c.UserId == token.UserId).ToListAsync();
            }

            var result = await IncomeVerificationsPost.verificationPost(income,token, ExistSalary, ListIncome, IncomeExists);

            if (result.Result) 
            {
                var percentage = await OperationIncome.operationIncome(ExistSalary, income.Money);

                var newIncome = new Income()
                {
                    UserId = token.UserId,
                    Token = token.Token,
                    NameIncome = income.NameIncome,
                    Money = income.Money,
                    AddedDate = DateTime.UtcNow,
                    IsExpired = DateTime.UtcNow.AddDays(30),
                    DescriptionIncome = income.DescriptionIncome,
                    Percentage = percentage.PercentageIncome

                };

                ExistSalary.Salary += newIncome.Money;
                ExistSalary.Percentage = percentage.PercentageSalary;
                await _context.Incomes.AddAsync(newIncome);
                await _context.SaveChangesAsync();

                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("DeleteIncome")]
        public async Task<IActionResult> Delete(IncomeRequestDelete income)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == income.Token);

            MySalary? ExistSalary = null; List<Income>? incomeList = null; Income? incomeDelete = null;

            if (token != null)
            {
                incomeList = await _context.Incomes.Where(i => i.UserId == token.UserId).ToListAsync();
                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == token.UserId);

                foreach (var IncomeDeleteList in incomeList)
                {
                    if (IncomeDeleteList.NameIncome == income.NameIncome)
                        incomeDelete = IncomeDeleteList;
                }
            }
                
            var result = await IncomeVerificationsDelete.verificationDelete(token,ExistSalary,incomeList,incomeDelete);

            if(result.Result) 
            {
                var percentage = await OperationIncome.operationIncome(ExistSalary, incomeDelete.Money);

                ExistSalary.Salary -= incomeDelete.Money;
                ExistSalary.Percentage -= percentage.PercentageIncome;
                _context.Incomes.Remove(incomeDelete);
                await _context.SaveChangesAsync();

                return Ok(result);
            }
            
            return BadRequest(result);
        }
        [HttpPut("UpdateIncome")]
        public async Task<IActionResult> Put(IncomeRequestPost income)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == income.Token);

            Income? incomeUpdate = null;  MySalary? ExistSalary = null;  List<Income>? incomeList = null;

            if (token != null)
            {
                incomeList = await _context.Incomes.Where(l => l.UserId == token.UserId).ToListAsync();
                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == token.UserId);

                foreach (var incomeName in incomeList)
                {
                    if (incomeName.NameIncome == income.NameIncome)
                        incomeUpdate = incomeName;
                }
            }

            var result = await IncomeVerificationsPut.verificationPut(token,income,incomeUpdate,incomeList);

            if (result.Result)
            {
                var percentage = await OperationIncome.operationIncome(ExistSalary, income.Money);
                var totalIncomes = incomeUpdate.Money - income.Money;
                var totalPercentage = incomeUpdate.Percentage - percentage.PercentageIncome;

                ExistSalary.Salary -= totalIncomes;
                ExistSalary.Percentage -= totalPercentage;

                incomeUpdate.Money = income.Money;
                incomeUpdate.Percentage = percentage.PercentageIncome;
                incomeUpdate.DescriptionIncome = income.DescriptionIncome;

                await _context.SaveChangesAsync();

                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
