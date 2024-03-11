using API.FINANCE.API.Configuration;
using API.FINANCE.Data;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.Common;
using API.FINANCE.Shared.DTOs.LoginAndRegisterRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
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
        private readonly TokenValidationParameters _tokenValidationParameters;

        public LoginController(UserManager<IdentityUser> userManager, IOptions<JwtConfig> JwtConfig,APIFinanceContext context, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _jwtConfig = JwtConfig.Value;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
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

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResult
                {
                    Errors=new List<string> { "Invalid parameters"},
                    Result = false
                });
            var results = VerifyAndGenerateTokenAsync(tokenRequest);

            if (results == null)
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { "Invalid Token"},
                    Result=false          
                });

            return Ok(await results);
        }

        private async Task<AuthResult> VerifyAndGenerateTokenAsync(TokenRequest tokenRequest)
        {
            var jwtTokenHandler=new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;

                var tokenBeingVerified=jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);

                    if (!result || tokenBeingVerified == null)
                        throw new Exception("Invalid Token");
                }

                var utcExpiryDate = long.Parse(tokenBeingVerified.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(utcExpiryDate).UtcDateTime;
                if (expiryDate < DateTime.UtcNow)
                    throw new Exception("Token Expired");

                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                    throw new Exception("Invalid Token");

                if (storedToken.IsUsed  || storedToken.IsRevoked)
                    throw new Exception("Invalid Token");

                var jti = tokenBeingVerified.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                if(jti != storedToken.JwtId)
                    throw new Exception("Invalid Token");

                if(storedToken.IsExpired < DateTime.UtcNow)
                    throw new Exception("Token Expired");

                storedToken.IsUsed= true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                return await GenerateToken(dbUser);

                
            }
            catch (Exception e)
            {
                var message = e.Message == "Invalid Token" || e.Message == "Token Expired" ? e.Message : "Internal Server Error";

                return new AuthResult() { Result=false, Errors=new List<string> { message } };
                throw;
            }
        }
    }
}

