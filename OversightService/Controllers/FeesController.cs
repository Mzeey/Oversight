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
    public class FeesController : ControllerBase
    {
        private readonly IFeesRepository _repo;
        public FeesController(IFeesRepository repo){
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<Fee>))]
        public async Task<IEnumerable<Fee>> GetFees(){
            return await _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetFee))]
        [ProducesResponseType(200, Type=typeof(Fee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFee(int id){
            Fee fee = await _repo.RetrieveAsync(id);
            if(fee is null){
                return NotFound();
            }
            return Ok(fee);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200, Type = typeof(Fee))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFee([FromBody] Fee fee){
            if(fee is null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            fee.IsActive = true;
            fee.DateCreated = DateTime.Now;
            fee.DateModified = fee.DateCreated;
            Fee newFee = await _repo.CreateAsync(fee);
            return CreatedAtRoute(
                routeName: nameof(GetFee),
                routeValues: new{id = newFee.Id},
                value: new{
                    NewFee= newFee,
                    Message = $"{newFee.Title} was created Successfully",
                    IsCreated = true
                }
            );
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] Fee fee){
            if(fee is null || fee.Id != id){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            fee.DateModified = DateTime.Now;
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }

            await _repo.UpdateAsync(id, fee);
            return Ok(new{
                Modified = fee,
                isUpdated = true,
                Message = $"Fee '{fee.Id}' was updated successfully"
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
                        Message = $"Fee '{existing.Title}' with id: '{existing.Id}' was found but faild to delete",
                        isFound = true,
                        isDeleted = false
                    }
                    
                );
            }

        }

    }
}