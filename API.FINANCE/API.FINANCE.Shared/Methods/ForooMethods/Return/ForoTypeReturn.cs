using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.ForooMethods.Return
{
    public class ForoTypeReturn
    {
        public string NameUser { get; set; }
        public DateTime AddedDate { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
        public int ForoId { get; set; }
    }
}
