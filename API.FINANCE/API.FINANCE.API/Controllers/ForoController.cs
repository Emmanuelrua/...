using API.FINANCE.Data;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using API.FINANCE.Shared.DTOs.ForoRequest;
using API.FINANCE.Shared.Methods.ForooMethods.Return;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FINANCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForoController : ControllerBase
    {
        private readonly APIFinanceContext _context;

        public ForoController(APIFinanceContext context)
        {
            _context = context;
        }

        [HttpPost("AddMessage")]
        public async Task<IActionResult> Post(ForoRequest foroRequest)
        {
            var User = await _context.RefreshTokens.FirstOrDefaultAsync(U => U.Token == foroRequest.Token);
            if (User != null) 
            {
                var nameUser = await _context.Users.FirstOrDefaultAsync(N => N.Id == User.UserId);
                if (nameUser != null)
                {
                    var newPost = new Foro()
                    {
                        UserId = nameUser.Id,
                        Message = foroRequest.Message,
                        Token = User.Token,
                        NameUser = nameUser.UserName,
                        AddedDate = DateTime.Now
                    };
                    await _context.Foro.AddAsync(newPost);
                    await _context.SaveChangesAsync();

                    return Ok(new AuthResultForo() { Result = true });
                }
            }
            return BadRequest(new AuthResultForo() { Result = false, Errors = new List<string>() { "Username does not exist" }});
        }
        [HttpGet]
        public async Task<IEnumerable<ForoTypeReturn>> Get()
        {
            return await _context.Foro.Select(x => new ForoTypeReturn
            {
                NameUser = x.NameUser,
                Message = x.Message,
                AddedDate = x.AddedDate,
                ForoId = x.ForoId,

            }).ToListAsync();

        }
        [HttpDelete("{ForoId}")]
        public async Task<IActionResult> Delete(int ForoId)
        {
            if (ForoId == null) return NotFound();

            var ExistMessage = await _context.Foro.FirstOrDefaultAsync(E => E.ForoId == ForoId);

            if (ExistMessage == null) return BadRequest(new AuthResultForo() { Result = false, Errors = new List<string>() { "Message does not exist" } });

            _context.Foro.Remove(ExistMessage);
            await _context.SaveChangesAsync();

            return Ok(new AuthResultForo() { Result = true });
        }
    }
} 
