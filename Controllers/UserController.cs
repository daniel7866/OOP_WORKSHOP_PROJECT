using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 'https://localhost:port/api/user'
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;

        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }

        [HttpGet("id/{id}")]
        public ActionResult<IEnumerable<User>> GetUserById(int id)
        {
            var user = _repo.GetUserById(id);
            if (user is null)
                return NotFound();
            ReadUserDto dto = new ReadUserDto()
            {
                Name = user.Name,
                Email = user.Email,
                Followers = (List<int>)_repo.GetFollowers(id),
                Following = (List<int>)_repo.GetFollowing(id)
            };

            return Ok(dto);
        }

        [HttpGet("email/{email}")]
        public ActionResult<IEnumerable<User>> GetUserByEmail(string email)
        {
            var user = _repo.GetUserByEmail(email);
            if (user is null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> AddUser(User user)
        {
            var result = _repo.AddUser(user);

            if (result)
                return Ok(user);
            else
                return BadRequest();
        }
    }
}
