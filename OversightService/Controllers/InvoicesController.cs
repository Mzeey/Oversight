using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mzeey.Shared;
using OversightService.Repositories;

namespace OversightService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoicesRepository _repo;
        private readonly IUsersRepository _usersRepo;
        public InvoicesController(IInvoicesRepository repo, IUsersRepository usersRepo){
            _repo = repo;
            _usersRepo = usersRepo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<Invoice>))]
        public async Task<IEnumerable<Invoice>> GetInvoices(){
            return await _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetInvoice))]
        [ProducesResponseType(200, Type=typeof(Invoice))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetInvoice(int id){
            Invoice invoice = await _repo.RetrieveAsync(id);
            if(invoice is null){
                return NotFound();
            }
            return Ok(invoice);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200, Type = typeof(Invoice))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice, int userId){
            if(invoice is null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            User user = (await _usersRepo.RetrieveAsync(userId));
            if(user is null){
                return BadRequest(
                    new{
                        Message = $"User with the id:'{userId}' was not found"
                    }
                );
            }
            invoice.User = user;
            invoice.DatePaid = DateTime.Now;
            Invoice newInvoice = await _repo.CreateAsync(invoice);
            return CreatedAtRoute(
                routeName: nameof(GetInvoice),
                routeValues: new{id = newInvoice.Id},
                value: new{
                    NewFee= newInvoice,
                    Message = $"'{newInvoice.Id}' was created Successfully",
                    IsCreated = true
                }
            );
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice){
            if(invoice is null || invoice.Id != id){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }

            await _repo.UpdateAsync(id, invoice);
            return Ok(new{
                Modified = invoice,
                isUpdated = true,
                Message = $"Fee '{invoice.Id}' was updated successfully"
            });
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id){
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }
            bool? isDeleted = await _repo.DeleteAsync(id);
            if(isDeleted.HasValue && isDeleted.Value){
                return Ok( 
                    new{
                        Messages = "Deleted Successfully",
                        isFound = true, 
                        isDeleted = true
                    }
                );
            }else{
                return BadRequest(
                    new {
                        Message = $"Invoice with id: '{existing.Id}' was found but faild to delete",
                        isFound = true,
                        isDeleted = false
                    }
                    
                );
            }

        }
    }
}