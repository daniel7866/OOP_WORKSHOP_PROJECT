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
        public ActionResult<User> AddUser([FromForm] WriteUserDto dto)
        {
            User user = new User()
            {
                Email = dto.Email,
                Password = dto.Password,
                Name = dto.Name
            };
            bool result;
            if(dto.file is null)//if there is no profile picture
                result = _repo.AddUser(user);
            else // there is a profile picture to save
            {
                try
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    //string newFilePath = path + DateTime.Now.GetHashCode() + dto.file.FileName;
                    //using (FileStream fileStream = System.IO.File.Create(newFilePath))
                    //{
                    //    dto.file.CopyTo(fileStream);
                    //    fileStream.Flush();
                    //}

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
    }
}
