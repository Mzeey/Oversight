using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public interface IUsersRepository
    {
        Task<User> RetrieveAsync(int id);
        Task<IEnumerable<User>> RetrieveAllAsync();
        Task<User> UpdateAsync(int id, User u);
        Task<User> CreateAsync(User u);
        Task<bool?> DeleteAsync(int id);
    }
}