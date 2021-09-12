using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Helpers;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace OOP_WORKSHOP_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 'https://localhost:port/api/user'
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JwtService _jwtService;

        public UserController(IUserRepo repo, IWebHostEnvironment webHostEnvironment, JwtService jwtService)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
        }

        [HttpGet]
      /*  [Authorize]*/
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
            if (user is null)
                return NotFound();
            ReadUserDto dto = MapToReadUserDto(user);
            return Ok(dto);
        }

        [HttpGet("search/{searchInput}")]
        public ActionResult<User> SearchUser(string searchInput)
        {
            var result = _repo.SearchUser(searchInput);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<User>> AddUserAsync(/*[FromForm]*/ WriteUserDto dto)
        {
            User user = MapToUser(dto);
            bool result;
            WriteUserDto cpy = dto;
            if (dto.file is null && dto.ImagePath is null)//if there is no profile picture
                result = _repo.AddUser(user);
            else // there is a profile picture to save
            {
                try
                {
                    //string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

                    //user.ImagePath = Services.SaveImage(dto.file, path);
                    //string dummyfile = @"C:\Users\danie\Downloads\h.jpg";
                    user.ImagePath = await Services.SaveImageAWSAsync(dto.ImagePath);
                    result = _repo.AddUser(user);
                }
                catch (Exception e) {
                    result = false;
                    Console.WriteLine(e);
                    return BadRequest(e);
                }
            }
            if (result)
                return Ok(user);
            else
                return BadRequest();
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(/*[FromForm]*/ WriteUserDto dto)
        {
            User user = MapToUser(dto);
            bool result;
            if (dto.file is null && dto.ImagePath is null)//if there is no profile picture
                try {
                    result = _repo.AddUser(user);
                } catch (Exception e)
                {
                    return BadRequest(e.InnerException.Message.ToString());
                }
            else // there is a profile picture to save
            {
                try
                {
                    //string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

                    //user.ImagePath = Services.SaveImage(dto.file, path);
                    //string dummyfile = @"C:\Users\danie\Downloads\h.jpg";
                    user.ImagePath = await Services.SaveImageAWSAsync(dto.ImagePath);
                    result = _repo.AddUser(user);
                }
                catch (Exception e)
                {
                    result = false;
                    Console.WriteLine(e);
                    return BadRequest(e);
                }
            }
            if (result)
                return Created("success", user);
            else
                return BadRequest();
        }

        [HttpPost("login")]
        public ActionResult<User> Login(LoginDto dto)
        {
            var user = _repo.GetUserByEmail(dto.Email);
            if (user == null) return BadRequest(new { message = "Invalid Credentials" });
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new { message = "Invalid Credentials" });

            var jwt = _jwtService.generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });


            return Ok(user);

        }

        [HttpPost("getUser")] //receives the JWT token and returns the user
        public IActionResult GetUserByToken()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                int userId = _jwtService.GetUserId(jwt);
                var user = _repo.GetUserById(userId);

                return Ok(user);
            }

            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logOut")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            { messege = "Succesefuly logged out" });
        }

        [HttpPost("message")]
        public IActionResult SendMessage(Message message)
        {
            message.DateSent = DateTime.Now;
            try
            {
                var jwt = Request.Cookies["jwt"];
                message.SenderId = _jwtService.GetUserId(jwt);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }

            if (_repo.AddMessage(message))
                return Created("Message sent successfully",message);
            else
                return BadRequest();
        }

        [HttpGet("message/{userId}")]
        public ActionResult<IEnumerable<Message>> GetMessages(int userId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
            return Ok(_repo.GetMessages(userId));
        }

        [HttpPost("follow/{followedId}")]
        public ActionResult Follow(int followedId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                int followingId = _jwtService.GetUserId(jwt);

                return Ok(_repo.FollowUser(followingId,followedId));
            }

            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

        }

        [HttpDelete("unfollow/{followedId}")]
        public ActionResult Unfollow(int followedId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                int followingId = _jwtService.GetUserId(jwt);

                return Ok(_repo.UnfollowUser(followingId, followedId));
            }

            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

        }

        [HttpGet("getFollowers/id/{userId}")]
        public ActionResult GetFollowers(int userId)
        {
            var followers = _repo.GetFollowers(userId);
            return Ok(followers);
        }

        [HttpGet("getFollowing/id/{userId}")]
        public ActionResult GetFollowing(int userId)
        {
            var following = _repo.GetFollowing(userId);
            return Ok(following);
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
            if (!IsValidEmail(dto.Email))
                throw new Exception("Invalid email");
            if (!IsValidPassword(dto.Password))
                throw new Exception("Password must be at least 4 characters long and include one letter and one number");

            return new User()
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                ImagePath = dto.ImagePath
            };
        }

        private bool IsValidEmail(String email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool IsValidPassword(String password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasChar = new Regex(@"[a-zA-Z]+");
            var hasMinimum4Chars = new Regex(@".{4,}");

            var isValidated = hasNumber.IsMatch(password) && hasChar.IsMatch(password) && hasMinimum4Chars.IsMatch(password);

            return isValidated;
        }
    }
}
