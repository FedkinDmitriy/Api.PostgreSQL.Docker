using Data.Repositories.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations
{
    public class ClientService : ICRUDable<Client, int>
    {

        private readonly Context _context;

        public ClientService(Context context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clients.ToListAsync(cancellationToken);
        }

        public async Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Clients.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Client entity, CancellationToken cancellationToken = default)
        {
            _context.Clients.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Client entity, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Clients.FindAsync(entity.Id, cancellationToken);
            if (existing is not null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var client = await _context.Clients.FindAsync(id, cancellationToken);
            if (client is not null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
