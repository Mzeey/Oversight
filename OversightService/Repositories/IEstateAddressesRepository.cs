using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public interface IEstateAddressesRepository
    {
        Task<EstateAddress> RetrieveAsync(int id);
        Task<IEnumerable<EstateAddress>> RetrieveAllAsync();
        Task<EstateAddress> UpdateAsync(int id, EstateAddress es);
        Task<EstateAddress> CreateAsync(EstateAddress es);
        Task<bool?> DeleteAsync(int id);
    }
}