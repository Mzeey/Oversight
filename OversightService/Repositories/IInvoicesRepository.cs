using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public interface IInvoicesRepository
    {
        Task<Invoice> RetrieveAsync(int id);
        Task<IEnumerable<Invoice>> RetrieveAllAsync();
        Task<Invoice> UpdateAsync(int id, Invoice invoice);
        Task<Invoice> CreateAsync(Invoice invoice);
        Task<bool?> DeleteAsync(int id);
    }
}