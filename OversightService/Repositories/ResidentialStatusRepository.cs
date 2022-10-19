using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public class ResidentialStatusRepository : IResidentialStatusRepository
    {   
        private readonly Oversight _db;
        private static ConcurrentDictionary<int, ResidentialStatus> _residentialStatusCache;
        public ResidentialStatusRepository(Oversight db){
            _db = db;
            if(_residentialStatusCache is null){
                _residentialStatusCache = new ConcurrentDictionary<int, ResidentialStatus>(
                    _db.ResidentialStatuses.ToDictionary(rs => rs.Id)
                );
            }
        }

        private ResidentialStatus updateCache(int id, ResidentialStatus rs){
            ResidentialStatus oldRs;
            if(_residentialStatusCache.TryGetValue(id, out oldRs)){
                if(_residentialStatusCache.TryUpdate(id, rs, oldRs)){
                    return rs;
                }
            }
            return null;
        }

        public async Task<ResidentialStatus> CreateAsync(ResidentialStatus rs)
        {
            EntityEntry<ResidentialStatus> added = await _db.ResidentialStatuses.AddAsync(rs);
            int affected = await _db.SaveChangesAsync();
            
            if(affected ==1){
                return _residentialStatusCache.AddOrUpdate(rs.Id, rs, updateCache);
            }else{
                return null;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            ResidentialStatus rs =  _db.ResidentialStatuses.Find(id);
            _db.ResidentialStatuses.Remove(rs);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return _residentialStatusCache.TryRemove(id, out rs);
            }else{
                return null;
            }
        }

        public Task<IEnumerable<ResidentialStatus>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<ResidentialStatus>>(() => _residentialStatusCache.Values);
        }

        public Task<ResidentialStatus> RetrieveAsync(int id)
        {
            return Task.Run(()=> {
                ResidentialStatus rs;
                _residentialStatusCache.TryGetValue(id, out rs);
                return rs;
            });
        }

        public async Task<ResidentialStatus> UpdateAsync(int id, ResidentialStatus rs)
        {
            _db.ResidentialStatuses.Update(rs);
            int affected = await _db.SaveChangesAsync();

            if(affected == 1){
                return updateCache(id, rs);
            }
            return null;
        }
    }
}