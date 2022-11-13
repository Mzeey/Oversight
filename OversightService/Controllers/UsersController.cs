using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mzeey.Shared;
using OversightService.Repositories;
using Microsoft.EntityFrameworkCore;


namespace OversightService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repo;
        private readonly IResidentialStatusRepository _residentialStatusRepo;
        public UsersController(IUsersRepository repo, IResidentialStatusRepository residentialStatusRepo){
            _repo = repo;
            _residentialStatusRepo = residentialStatusRepo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<User>))]
        public async Task<IEnumerable<User>> GetUsers(){
            return await _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        [ProducesResponseType(200, Type=typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(int id){
            User u = await _repo.RetrieveAsync(id);
            if(u is null){
                return NotFound();
            }
            return Ok(u);
        }

        [HttpGet("Login")]
        [ProducesResponseType(200, Type=typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LoginUser(string email, string password){
            User u = (await _repo.RetrieveAllAsync())
                        .FirstOrDefault(user => user.Email == email && user.HashedPassword == password);
            if (u is null){
                return NotFound();
            }
            return Ok(u);
        }

        [HttpGet("CheckPassword", Name = nameof(CheckUserPassword))]
        [ProducesResponseType(200, Type=typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CheckUserPassword(int id, string password){
            User u = await _repo.RetrieveAsync(id);
            if (u.HashedPassword.Equals(password)){
                return Ok(new{
                    userId = u.Id,
                    isMatch = true,
                    Message = $"{u.Id}'s password is a match"
                });
            }else{
                return NotFound(new{
                    userId = u.Id,
                    isMatch = false,
                    Message = $"{u.Id}'s password is NOT a match"
                });
            }         
        }

        [HttpGet("DoesEmailExist")]
        public async Task<IActionResult> CheckEmailExist(string email){
            List<User> users = (await _repo.RetrieveAllAsync()).Where(u => u.Email == email).ToList();
            if(users.Count == 0){
                return NotFound(
                    new {
                        Message = $" A User with '{email}' email does not exist",
                        isFound = false
                    }
                );
            }else {
                return Ok(
                    new{
                        Message = $"'{email}' Already Exists",
                        isFound = true
                    }
                );
            }
        }

        [HttpPost("Register")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterUser(string userType, [FromBody]User user, string accessControl = "5"){
            if(user is null){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            ResidentialStatus  residentialStatus = (await _residentialStatusRepo.RetrieveAllAsync())
                                    .FirstOrDefault(rs => rs.Title.ToLower().Equals(userType.ToLower()));
            user.ResidentStatus = residentialStatus;
            user.StatusControl = Convert.ToByte(accessControl);
            user.IsActive = true;
            user.DateCreated = DateTime.Now;
            user.DateModified = user.DateCreated;
            User newUser = await _repo.CreateAsync(user);
            return CreatedAtRoute(
                routeName: nameof(GetUser),
                routeValues: new{id = newUser.Id},
                value: new{
                    NewUser= newUser,
                    Message = $" {newUser.FirstName} {newUser.LastName} was created Successfully",
                    IsCreated = true
                }
            );
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User u){
            if(u is null || u.Id != id){
                return BadRequest();
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);   
            }
            u.DateModified = DateTime.Now;
            var existing = await _repo.RetrieveAsync(id);
            if(existing is null){
                return NotFound();
            }

            await _repo.UpdateAsync(id, u);
            return  Ok(new{
                Modified = u,
                isUpdated = true,
                Message = $"User '{u.Id}' was updated successfully"
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
                        Message = $"User '{existing.FirstName} {existing.LastName}' with id: '{existing.Id}' was found but faild to delete",
                        isFound = true,
                        isDeleted = false
                    }
                    
                );
            }

        }
    }
}