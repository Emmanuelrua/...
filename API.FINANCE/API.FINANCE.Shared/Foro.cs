using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared
{
    public class Foro
    {
        [Key]
        public int ForoId { get; set; }
        public string UserId { get; set; }
        public string NameUser { get; set; }
        public string Token { get; set; }
        public DateTime AddedDate { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
    }
}
