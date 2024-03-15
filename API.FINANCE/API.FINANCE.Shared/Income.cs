using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared
{
    public class Income
    {
        [Key]
        public int IncomeId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NameIncome { get; set; }
        public int Money { get; set; }
        public string DescriptionIncome { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime IsExpired { get; set; }
        public double Percentage { get; set; }
    }
}
