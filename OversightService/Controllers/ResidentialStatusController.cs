using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OversightService.Repositories;
using Mzeey.Shared;

namespace OversightService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResidentialStatusController : ControllerBase
    {
        private readonly IResidentialStatusRepository _repo;

        public ResidentialStatusController(IResidentialStatusRepository repo){
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ResidentialStatus>))]
        public async Task<IEnumerable<ResidentialStatus>> GetResidentialStatuses(){
            return await _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetResidentialStatus))]
        [ProducesResponseType(200, Type = typeof(ResidentialStatus))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetResidentialStatus(int id){
            ResidentialStatus rs = await _repo.RetrieveAsync(id);
            if(rs is null){
                return NotFound();
            }
            return Ok(rs);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResidentialStatus))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ResidentialStatus rs){
            if (rs is null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            ResidentialStatus newRs = await _repo.CreateAsync(rs);
            return CreatedAtRoute(
                routeName: nameof(GetResidentialStatus),
                routeValues: new {id = newRs.Id},
                value: newRs
            );

        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ResidentialStatus rs){
            if(rs is null || rs.Id != id){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }
             
            await _repo.UpdateAsync(id, rs);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id){
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }
            bool? isDeleted = await _repo.DeleteAsync(id);
            if(isDeleted.HasValue && isDeleted.Value){
                return new NoContentResult();
            }else{
                return BadRequest(
                    $"Residential status '{existing.Title}' was found but faild to delete"
                );
            }

        }
    }
}