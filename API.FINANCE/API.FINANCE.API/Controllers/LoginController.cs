using API.FINANCE.API.Configuration;
using API.FINANCE.Data;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.Common;
using API.FINANCE.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.FINANCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly APIFinanceContext _context;

        public LoginController(UserManager<IdentityUser> userManager, IOptions<JwtConfig> JwtConfig,APIFinanceContext context)
        {
            _userManager = userManager;
            _jwtConfig = JwtConfig.Value;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            //check if user exists
            var existingUser = await _userManager.FindByNameAsync(request.NameUser);

            if (existingUser == null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() { "Invalid Paylod" },
                    Result = false
                });

            if (!existingUser.EmailConfirmed)
                return BadRequest(new AuthResult
                {
                    Errors = new List<string>() { "Email needs to be confirmed" },
                    Result = false
                });

            var checkUserAndPass = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (!checkUserAndPass)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() { "Invalid Credentials" },
                    Result = false
                });

            return Ok(await GenerateToken(existingUser));
        }


        private async Task<AuthResult> GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var Key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new ClaimsIdentity(new[]
                {
                            new Claim("Id",user.Id),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                        })),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256)
            };

            var Token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken=jwtTokenHandler.WriteToken(Token);

            var refreshToken = new RefreshToken
            {
                JwtId = Token.Id,
                Token = RandomGenerator.GenerateRandomString(23),
                AddedDate = DateTime.UtcNow,
                IsExpired = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id

            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();


            return new AuthResult
            {
                Token = jwtToken,
                refreshToken = refreshToken.Token,
                Result = true
            }; ;

        }
    }
}

