using Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal OrderSum { get; set; }
        public DateTime OrdersDateTime { get; set; }
        public OrderStatus Status { get; set; }
        public int ClientId { get; set; }

        public Client? Client { get; set; }
    }
}
