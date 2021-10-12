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
using Microsoft.Data.SqlClient;

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
            ReadUserDto dto = Services.MapToReadUserDto(user,_repo);
            return Ok(dto);
        }

        [HttpGet("email/{email}")]
        public ActionResult<User> GetUserByEmail(string email)
        {
            var user = _repo.GetUserByEmail(email);
            if (user is null)
                return NotFound();
            ReadUserDto dto = Services.MapToReadUserDto(user,_repo);
            return Ok(dto);
        }

        [HttpPost("search/")]
        public ActionResult<User> SearchUser(dynamic searchInput)
        {
            string search = searchInput.GetProperty("search").ToString();
            if (search == "")
                return Ok(new List<User>());
            
            var result = _repo.SearchUser(search);
            return Ok(result);
        }


        [HttpPost("register")]
        public ActionResult<User> RegisterUser(/*[FromForm]*/ WriteUserDto dto)
        {
            if (!ValidateEmail(dto.Email))
                return BadRequest("Invalid email");
            if (!ValidatePassword(dto.Password))
                return BadRequest("Password must be at least 4 characters long and include one letter and one number");

            User user = Services.MapToUser(dto);
            try
            {
                _repo.AddUser(user);
                return Created("success", user);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException; // find the most inner exception
                if(e is SqlException sqlException && sqlException.Number == 2601) // code for duplicate key(The only duplication that could happen is the email)
                {
                    return BadRequest("Email is already used!");
                }
                return BadRequest(e.Message);//else return 
            }
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

        [HttpGet("messages/")]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                var userId = _jwtService.GetUserId(jwt);
                return Ok(_repo.GetMessages(userId).OrderBy(x => x.DateSent));
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        /**
        This method will return the list of all the users we have messages with
        */
        [HttpGet("messages/users")]
        public ActionResult<IEnumerable<int>> GetMessagedUsers()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                var userId = _jwtService.GetUserId(jwt);
                return Ok(_repo.GetMessagedUsers(userId));
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        /**
        This method will get a user id with whom we have messages with, and it will return all those messages
        **/
        [HttpGet("messages/user/{userId}")]
        public ActionResult<IEnumerable<Message>> GetMessagesFromUser(int userId){
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                var loggedUserId = _jwtService.GetUserId(jwt);
                return Ok(_repo.GetMessagesFromUser(loggedUserId, userId));
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
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

        private bool ValidateEmail(String email) { 
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

        private bool ValidatePassword(String password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasChar = new Regex(@"[a-zA-Z]+");
            var hasMinimum4Chars = new Regex(@".{4,}");

            var isValidated = hasNumber.IsMatch(password) && hasChar.IsMatch(password) && hasMinimum4Chars.IsMatch(password);

            return isValidated;
        }
    }
}
