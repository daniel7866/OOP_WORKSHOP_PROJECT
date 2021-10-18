using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Helpers;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace OOP_WORKSHOP_PROJECT.Controllers
{

    /*
    This class will handle all http requests regarding users.
    This includes:
    registering, loggin in, edit details ,viewing users, following them and message them.

    The route of the http requests is: "http://ip_address:port/api/user/{a path for a specific function}"
    */
    [ApiController]
    [Route("api/[controller]")] // 'https://localhost:port/api/user'
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo; // the repository in which the users are stored
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JwtService _jwtService; // json web tokens service for verifying indentity

        //This is the default empty profile picture every new user will have untill he uploads a new one
        private readonly string EMPTY_PICTURE = "https://firebasestorage.googleapis.com/v0/b/oop-project-5cde7.appspot.com/o/images%2Fblank-profile-picture-973460_1280.png%20%2B%201634107421204?alt=media&token=2b476f59-ba06-45ad-ba3b-92025e7863f0";

        //get the properties using dependency injection in ConfigureServices() in Startup.CS file
        public UserController(IUserRepo repo, IWebHostEnvironment webHostEnvironment, JwtService jwtService)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
        }
        [HttpGet()]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }

        [HttpGet("id/{id}")]
        public ActionResult<ReadUserDto> GetUserById(int id)
        {
            var user = _repo.GetUserById(id);
            if (user is null)
                return NotFound();
            ReadUserDto dto = Services.MapToReadUserDto(user,_repo);
            return Ok(dto);
        }

        [HttpGet("email/{email}")]
        public ActionResult<ReadUserDto> GetUserByEmail(string email)
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


        /*
            This method will register a new user into the system.
            It will get a name, an email and a password.
            It will check that the email is valid and free(not being used).
            It will check that the password is valid.
        */
        [HttpPost("register")]
        public ActionResult<User> RegisterUser(/*[FromForm]*/ WriteUserDto dto)
        {
            if (!ValidateEmail(dto.Email))
                return BadRequest("Invalid email");
            if (!ValidatePassword(dto.Password))
                return BadRequest("Password must be at least 4 characters long and include one letter and one number");

            User user = Services.MapToUser(dto);// convert dto to a User obejct to be stored in the database

            user.ImagePath = EMPTY_PICTURE;//set a default profile picture for the user
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

        /*
            Update user details:
            A user will try to update one detail at a time and it could be one of the followings:

            *His name
            *His profile picture
            *His password

            In order to chane his password he must also enter his old password and it must be verified.
        */
        [HttpPatch("update")]
        public ActionResult<User> UpdateUserInfo(UpdateUserDto newUserInfo)
        {
            User user = null;
            try{ //check for token to verify indentity
                user = GetUserByToken();
                if(user == null)
                    return Unauthorized();
            }
            catch(Exception e){ return Unauthorized();}

            //Check that the user is changing his password
            if(!String.IsNullOrEmpty(newUserInfo.OldPassword)){//if he entered his old password
                if (!BCrypt.Net.BCrypt.Verify(newUserInfo.OldPassword, user.Password))//validate it
                    return BadRequest(new { message = "Wrong Password" });
            }
            else{//if there is no old password he cannot switch to a new password - delete it if it's not emtpy
                newUserInfo.OldPassword = null;
                newUserInfo.Password = null;
            }

            if (!String.IsNullOrEmpty(newUserInfo.Email))//if he wants to change his email - validate it
            {
                if (!ValidateEmail(newUserInfo.Email))
                    return BadRequest(new { message = "Invalid email"});
                if(_repo.GetUserByEmail(newUserInfo.Email) != null){
                    return BadRequest(new { message = "Email already exists"});
                }
            }

            if (!String.IsNullOrEmpty(newUserInfo.Password))
            { 
                if (!ValidatePassword(newUserInfo.Password))
                    return BadRequest(new { message = "Password must be at least 4 characters long and include one letter and one number" }); 
            }

            try
            {
                _repo.UpdateUserInfo(newUserInfo,user.Id);
                return Ok(new { message = "Changes applied" });
            }

            catch(Exception e) { return BadRequest(e.InnerException.Message.ToString()); }

        }

        /*
            Login in to the system:
            validate the user's email and password.
            after successful validation generate a new json web token and send it to the user's cookies
        */
        [HttpPost("login")]
        public ActionResult<ReadUserDto> Login(LoginDto dto)
        {
            var user = _repo.GetUserByEmail(dto.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return BadRequest(new { message = "Invalid Credentials" });
            
            //After validating all the details, create a web token and send it to the user's cookies
            var jwt = _jwtService.generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            var readUserDto = Services.MapToReadUserDto(user, _repo);
            return Ok(readUserDto);
        }


        /*
            Logout from the system:
            Simply remove the web token from the user's cookies
        */
        [HttpPost("logOut")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            { messege = "Succesefuly logged out" });
        }

        /*
            Send a message to a user:

            get the sender id is the is the id from the token cookies.
            set the time of the message.
        */
        [HttpPost("message")]
        public IActionResult SendMessage(Message message)
        {
            message.DateSent = DateTime.Now; // set the time of the message

            try
            {
                var jwt = Request.Cookies["jwt"];
                message.SenderId = _jwtService.GetUserId(jwt); // get the sender id from the cookie token
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

        //Get all the messages the user has
        [HttpGet("messages/")]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            try
            {
                var jwt = Request.Cookies["jwt"]; //verify his indentity
                var token = _jwtService.Verify(jwt);
                var userId = _jwtService.GetUserId(jwt);
                return Ok(_repo.GetMessages(userId).OrderBy(x => x.DateSent)); // sort them from old to new
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        /**
            This method will return the list of all the users we have messages with.

            get the user's indentity from the cookies.
            get all the users he has messages with.
            convert them to a ReadUserDto list and return it.
        */
        [HttpGet("messages/users")]
        public ActionResult<IEnumerable<int>> GetMessagedUsers()
        {
            try
            {
                //verify indentity
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                var userId = _jwtService.GetUserId(jwt);
                
                //get users id list
                var usersIdList = _repo.GetMessagedUsers(userId);

                //convert them to users
                var users = new List<User>();
                foreach(var id in usersIdList){
                    users.Add(_repo.GetUserById(id));
                }

                //convert them to ReadUserDto
                var dtos = new List<ReadUserDto>();
                foreach(var user in users){
                    dtos.Add(Services.MapToReadUserDto(user, _repo));
                }

                return Ok(dtos);
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

        /*
            Get all the followers a user has
        */
        [HttpGet("getFollowers/id/{userId}")]
        public ActionResult GetFollowers(int userId)
        {
            var followers = _repo.GetFollowers(userId);
            return Ok(followers);
        }
        
        /*
            Get all the people the user follows
        */
        [HttpGet("getFollowing/id/{userId}")]
        public ActionResult GetFollowing(int userId)
        {
            var following = _repo.GetFollowing(userId);
            return Ok(following);
        }

        public User GetUserByToken()//extracts the User from the JWT token
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                int userId = _jwtService.GetUserId(jwt);
                var user = _repo.GetUserById(userId);

                return user;
            }

            catch (Exception e) { }

            return null;
        }

        private bool ValidateEmail(String email) {  //validate email using regular expressions
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

        //validate password using regular expressions
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
