using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public class FeesRepository : IFeesRepository
    {
        public readonly Oversight _db;
        private static ConcurrentDictionary<int, Fee> _feesCache;
        
        public FeesRepository(Oversight db){
            _db = db;
            if(_feesCache is null){
                _feesCache = new ConcurrentDictionary<int, Fee>(
                    _db.Fees.ToDictionary(f => f.Id)
                );
            }
        }

        private Fee updateCache(int id, Fee f){
            Fee oldFee;
            if(_feesCache.TryGetValue(id, out oldFee)){
                if(_feesCache.TryUpdate(id,f,oldFee)){
                    return f;
                }
            }
            return null;
        }

        public async Task<Fee> CreateAsync(Fee f)
        {
            EntityEntry<Fee> added = await _db.Fees.AddAsync(f);
            int affected = await _db.SaveChangesAsync();

            if(affected == 1){
                return _feesCache.AddOrUpdate(f.Id, f, updateCache);
            }else{
                return null;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Fee f = _db.Fees.Find(id);
            _db.Fees.Remove(f);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return _feesCache.TryRemove(id, out f);
            }else{
                return null;
            }
        }

        public Task<Fee> RetrieveAsync(int id)
        {
            return Task.Run(() =>{
                Fee f;
                _feesCache.TryGetValue(id, out f);
                return f;
            });
        }

        public Task<IEnumerable<Fee>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<Fee>>(
                () => _feesCache.Values
            );
        }

        public async Task<Fee> UpdateAsync(int id, Fee f)
        {
            _db.Fees.Update(f);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return updateCache(id, f);
            }
            return null;
        }
    }
}