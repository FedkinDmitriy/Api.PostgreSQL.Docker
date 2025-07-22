using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class OrderService : ICRUDable<Order, int>
    {

        private readonly Context _context;

        public OrderService(Context context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders.ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FindAsync(id, cancellationToken);
            if (order is not null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Orders.FindAsync(entity.Id, cancellationToken);
            if (existing is not null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
