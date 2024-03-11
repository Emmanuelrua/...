using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.DTOs.ForoRequest
{
    public class ForoRequest
    {
        public string Token { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
    }
}
