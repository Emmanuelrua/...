using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.DTOs.LoginAndRegisterRequest
{
    public class UserLoginRequestDto
    {
        [Required]
        public string NameUser { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
