using API.FINANCE.API.Configuration;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs;
using API.FINANCE.Shared.HtmlEmail;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace API.FINANCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RegisterController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            //verify if email exists
            var emailExists = await _userManager.FindByEmailAsync(request.EmailAddress);

            if (emailExists != null) return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                            {
                                "Email already exists"
                            }
            });
            var user = new IdentityUser()
            {
                Email = request.EmailAddress,
                UserName = request.NameUser,
                PhoneNumber = request.Phone,
                EmailConfirmed = false

            };

            var isCreated = await _userManager.CreateAsync(user, request.Password);

            if (isCreated.Succeeded) 

            {
                await SendVerificationEmail(user)
                ;
                return Ok(new AuthResult()
                {
                    Result = true
                });
            }
            else
            {
                var errors = new List<string>();
                foreach (var err in isCreated.Errors) errors.Add(err.Description);

                return BadRequest(new AuthResult()
                {
                    Errors = errors,
                    Result = false
                });

            }
        }

        [HttpGet("ConfirEmail")]
        public async Task<IActionResult> ConfirEmail(string userId, string code)
        {
            string? status= null;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return BadRequest(status = HtmlServicesThank.EstructureFalse());

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(status = HtmlServicesThank.EstructureFalse());

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                status =  HtmlServicesThank.EstructureTrue() ;
            }
            else
            {
                status =  HtmlServicesThank.EstructureFalse() ;
            }

            return Content(status, "text/html"); ;
        }
         
        private async Task SendVerificationEmail(IdentityUser user)
        {
            var verificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            verificationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationCode));

            var callbackURL = $"{Request.Scheme}://{Request.Host}{Url.Action("ConfirEmail", controller: "Register", new { UserId = user.Id, code = verificationCode })}";
            var emailBody = HtmlServicesRegister.Estructure(callbackURL);
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailBody);
        }

    }
}

