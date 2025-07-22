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

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть положительной")]
        public decimal OrderSum { get; set; }

        [Required]
        public DateTime OrdersDateTime { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client? Client { get; set; }
    }
}
