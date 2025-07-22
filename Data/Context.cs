using Data.Models;
using Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }


        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, FirstName = "Иван", LastName = "Иванов", DateOfBirth = new DateOnly(1987, 10, 10) },
            new Client { Id = 2, FirstName = "Пётр", LastName = "Петров", DateOfBirth = new DateOnly(1989, 11, 12) }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 11,
                    OrderSum = 1000,
                    OrdersDateTime = DateTime.SpecifyKind(new DateTime(2025, 4, 1, 12, 0, 0), DateTimeKind.Utc),
                    Status = 0,
                    ClientId = 1
                },
                new Order
                {
                    Id = 12,
                    OrderSum = 2000,
                    OrdersDateTime = DateTime.SpecifyKind(new DateTime(2025, 4, 1, 12, 0, 0), DateTimeKind.Utc),
                    Status = (OrderStatus)1,
                    ClientId = 2
                }
                );

        }

    }
}
