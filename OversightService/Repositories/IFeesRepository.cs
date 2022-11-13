using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public interface IFeesRepository
    {
        Task<Fee> RetrieveAsync(int id);
        Task<IEnumerable<Fee>> RetrieveAllAsync();
        Task<Fee> UpdateAsync(int id, Fee f);
        Task<Fee> CreateAsync(Fee f);
        Task<bool?> DeleteAsync(int id);
    }
}