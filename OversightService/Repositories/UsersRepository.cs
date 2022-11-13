using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly Oversight _db;
        private static ConcurrentDictionary<int, User> _usersCache;

        public UsersRepository(Oversight db){
            _db = db;
            if(_usersCache is null){
                _usersCache = new ConcurrentDictionary<int, User>(
                    _db.Users.ToDictionary(u => u.Id)
                );
            }
        }

        private User updateCache(int id, User u){
            User oldUser;
            if(_usersCache.TryGetValue(id, out oldUser)){
                if(_usersCache.TryUpdate(id,u,oldUser)){
                    return u;
                }
            }
            return null;
        }

        public async Task<User> CreateAsync(User u)
        {
            EntityEntry<User> added = await _db.Users.AddAsync(u);
            int affected = await _db.SaveChangesAsync();

            if(affected == 1){
                return _usersCache.AddOrUpdate(u.Id, u, updateCache);
            }else{
                return null;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            User u = _db.Users.Find(id);
            _db.Users.Remove(u);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return _usersCache.TryRemove(id, out u);
            }else{
                return null;
            }
        }

        public Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<User>>(
                () => _usersCache.Values
            );
        }

        public Task<User> RetrieveAsync(int id)
        {
            return Task.Run(() =>{
                User u;
                _usersCache.TryGetValue(id, out u);
                return u;
            });
        }

        public async Task<User> UpdateAsync(int id, User u)
        {
            _db.Users.Update(u);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return updateCache(id, u);
            }
            return null;
        }
    }
}