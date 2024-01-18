using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.DTOs.MySalaryRequest
{
    public class MySalaryRequest
    {
        public string Token { get; set; }
        public int Salary { get; set; }
        public string Message { get; set; }
    }
}
