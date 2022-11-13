using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.Shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace OversightService.Repositories
{
    public class InvoiceRepository : IInvoicesRepository
    {
        private readonly Oversight _db;
        private static ConcurrentDictionary<int, Invoice> _invoiceCache;

        public InvoiceRepository(Oversight db){
            _db = db;
            if(_invoiceCache is null){
                _invoiceCache = new ConcurrentDictionary<int, Invoice>(
                    _db.Invoices.ToDictionary( invoice => invoice.Id)
                );
            }
        }

        private Invoice updateCache(int id, Invoice invoice){
            Invoice oldInvoice;
            if(_invoiceCache.TryGetValue(id, out oldInvoice)){
                if(_invoiceCache.TryUpdate(id,invoice,oldInvoice)){
                    return invoice;
                }
            }
            return null;
        }
        public async Task<Invoice> CreateAsync(Invoice invoice)
        {
            EntityEntry<Invoice> added = await _db.Invoices.AddAsync(invoice);
            int affected = await _db.SaveChangesAsync();

            if(affected == 1){
                return _invoiceCache.AddOrUpdate(invoice.Id, invoice, updateCache);
            }else{
                return null;
            }
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Invoice invoice = _db.Invoices.Find(id);
            _db.Invoices.Remove(invoice);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return _invoiceCache.TryRemove(id, out invoice);
            }else{
                return null;
            }
        }

        public Task<IEnumerable<Invoice>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<Invoice>>(
                () => _invoiceCache.Values
            );
        }

        public Task<Invoice> RetrieveAsync(int id)
        {
            return Task.Run(() =>{
                Invoice invoice;
                _invoiceCache.TryGetValue(id, out invoice);
                return invoice;
            });
        }

        public async Task<Invoice> UpdateAsync(int id, Invoice invoice)
        {
            _db.Invoices.Update(invoice);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                return updateCache(id, invoice);
            }
            return null;
        }
    }
}