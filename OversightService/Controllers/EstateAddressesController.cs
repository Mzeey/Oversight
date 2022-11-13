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
    public class EstateAddressesController : ControllerBase
    {
        private readonly IEstateAddressesRepository _repo;
        
        public EstateAddressesController(IEstateAddressesRepository repo){
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<EstateAddress>))]
        public async Task<IEnumerable<EstateAddress>> GetEstateAddresses(){
            return await _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetEstateAddress))]
        [ProducesResponseType(200, Type=typeof(EstateAddress))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEstateAddress(int id){
            EstateAddress es = await _repo.RetrieveAsync(id);
            if(es is null){
                return NotFound();
            }
            return Ok(es);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200, Type = typeof(EstateAddress))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEstateAddress([FromBody] EstateAddress estateAddress){
            if(estateAddress is null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            EstateAddress newEstateAddress = await _repo.CreateAsync(estateAddress);
            return CreatedAtRoute(
                routeName: nameof(GetEstateAddress),
                routeValues: new{id = newEstateAddress.Id},
                value: new{
                    NewFee= newEstateAddress,
                    Message = $"{newEstateAddress.Title} was created Successfully",
                    IsCreated = true
                }
            );
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEstateAddress(int id, [FromBody] EstateAddress estateAddress){
            if(estateAddress is null || estateAddress.Id != id){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }

            await _repo.UpdateAsync(id, estateAddress);
            return Ok(new{
                Modified = estateAddress,
                isUpdated = true,
                Message = $"Estate Address '{estateAddress.Id}' was updated successfully"
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
                        Message = $"Estate Address '{existing.Title}' with id: '{existing.Id}' was found but faild to delete",
                        isFound = true,
                        isDeleted = false
                    }
                    
                );
            }

        }
    }
}