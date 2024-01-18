using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.DTOs.CategoriesRequest
{
    public class CategoryRequestDelete
    {
        public string Token { get; set; }
        public string NameCategory { get; set; }
    }
}
