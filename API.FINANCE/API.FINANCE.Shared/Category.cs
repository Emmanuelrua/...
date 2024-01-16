using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string NameCategory { get; set; }
        public string DescriptionCategory { get; set; }
    }
}
