using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public interface IResidentialStatusRepository
    {
        Task<ResidentialStatus> CreateAsync(ResidentialStatus rs);
        Task<IEnumerable<ResidentialStatus>> RetrieveAllAsync();
        Task<ResidentialStatus> RetrieveAsync(int id);
        Task<ResidentialStatus> UpdateAsync(int id, ResidentialStatus rs);
        Task<bool?> DeleteAsync(int id);
    }
}