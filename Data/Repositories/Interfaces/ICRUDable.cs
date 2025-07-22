using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface ICRUDable<TModel, TId>
    {
        Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task AddAsync(TModel entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TModel entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
    }
}
