using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.DTO
{
    public  class OrderDTO
    {
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть положительной")]
        public decimal OrderSum { get; set; }
        [Required]
        public DateTime OrdersDateTime { get; set; }
        [Required]
        public string Status { get; set; } = null!;
    }
}
