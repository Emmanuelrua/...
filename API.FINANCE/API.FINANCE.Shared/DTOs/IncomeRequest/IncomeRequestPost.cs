using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.DTOs.IncomeRequest
{
    public class IncomeRequestPost
    {
        public string Token { get; set; }
        public string NameIncome { get; set; }
        public int Money { get; set; }
        public string DescriptionIncome { get; set; }
    }
}
