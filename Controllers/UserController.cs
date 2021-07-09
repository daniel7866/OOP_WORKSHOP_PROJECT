using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 'https://localhost:port/api/user'
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserRepo repo, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }

        [HttpGet("id/{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _repo.GetUserById(id);
            if (user is null)
                return NotFound();
            ReadUserDto dto = MapToReadUserDto(user);
            return Ok(dto);
        }

        [HttpGet("email/{email}")]
        public ActionResult<User> GetUserByEmail(string email)
        {
            var user = _repo.GetUserByEmail(email);
            ReadUserDto dto = MapToReadUserDto(user);
            if (user is null)
                return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<User> AddUser([FromForm] WriteUserDto dto)
        {
            User user = MapToUser(dto);
            bool result;
            if(dto.file is null)//if there is no profile picture
                result = _repo.AddUser(user);
            else // there is a profile picture to save
            {
                try
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

                    user.ImagePath = Services.SaveImage(dto.file, path);
                    result = _repo.AddUser(user);
                }
                catch(Exception e) { 
                    result = false;
                    Console.WriteLine(e);
                }
            }
            if (result)
                return Ok(user);
            else
                return BadRequest();
        }

        private ReadUserDto MapToReadUserDto(User user)
        {
            return new ReadUserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Followers = (List<int>)_repo.GetFollowers(user.Id),
                Following = (List<int>)_repo.GetFollowing(user.Id)
            };
        }

        private User MapToUser(WriteUserDto dto)
        {
            return new User()
            {
                Email = dto.Email,
                Password = dto.Password,
                Name = dto.Name
            };
        }
    }
}
