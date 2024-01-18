using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared
{
    public class MySalary
    {
        [Key]
        public string SalaryId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public int  Salary { get; set; }
        public string Message { get; set; }
    }
}
