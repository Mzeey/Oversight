using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mzeey.Shared;

namespace OversightService.Repositories
{
    public class EstateAddressesRepository : IEstateAddressesRepository
    {
        private readonly Oversight _db;
        private static ConcurrentDictionary<int, EstateAddress> _estateaddressesCache;

        public EstateAddressesRepository(Oversight db){
            _db = db;
            if(_estateaddressesCache is null){
                _estateaddressesCache = new ConcurrentDictionary<int, EstateAddress>(
                    db.EstateAddresses.ToDictionary(es => es.Id)
                );
            }
        }

        private EstateAddress updateCache(int id, EstateAddress es){
            EstateAddress oldEstateAddress;
            if(_estateaddressesCache.TryGetValue(id, out oldEstateAddress)){
                if(_estateaddressesCache.TryUpdate(id, es, oldEstateAddress)){
                    return es;
                }
            }
            return null;
        }
        public Task<EstateAddress> RetrieveAsync(int id)
        {
            return Task.Run(() => {
                EstateAddress es;
                _estateaddressesCache.TryGetValue(id, out es);
                return es;
            });
        }

        public Task<IEnumerable<EstateAddress>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<EstateAddress>>(
                () => _estateaddressesCache.Values
            );
        }

        public async Task<EstateAddress> UpdateAsync(int id, EstateAddress es)
        {
            _db.EstateAddresses.Update(es);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return updateCache(id, es);
            }
            return null;
        }

        public async Task<EstateAddress> CreateAsync(EstateAddress es)
        {
            EntityEntry<EstateAddress> added = await _db.EstateAddresses.AddAsync(es);
            int affected = await _db.SaveChangesAsync();

            if(affected == 1){
                return _estateaddressesCache.AddOrUpdate(es.Id, es, updateCache);
            }else{
                return null;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            EstateAddress es = _db.EstateAddresses.Find(id);
            _db.EstateAddresses.Remove(es);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return _estateaddressesCache.TryRemove(id, out es);
            }else{
                return null;
            }
        }
    }
}