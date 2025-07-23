using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.DTO
{
    public  class OrderDTO
    {
        public int Id { get; set; }
        public decimal OrderSum { get; set; }
        public DateTime OrdersDateTime { get; set; }
        public string Status { get; set; } = null!;
    }
}
