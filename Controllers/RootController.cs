using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Authorization;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace OOP_WORKSHOP_PROJECT.Controllers
{

    /*
    This class will handle all http requests regarding root user.
    This includes:
    handle reported content on the application.

    The route of the http requests is: "http://ip_address:port/api/user/{a path for a specific function}"
    */
    [ApiController]
    [Route("api/[controller]")] // 'https://localhost:port/api/root'
    public class RootController : ControllerBase
    {
        private readonly IPostRepo _postRepo;

        private readonly IUserRepo _userRepo;

        private readonly IReportRepo _reportRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string EMPTY_PICTURE = "https://firebasestorage.googleapis.com/v0/b/oop-project-5cde7.appspot.com/o/images%2Fblank-profile-picture-973460_1280.png%20%2B%201634107421204?alt=media&token=2b476f59-ba06-45ad-ba3b-92025e7863f0";
        private readonly IAuthorize _jwtService; // json web tokens service for verifying indentity

        //get the properties using dependency injection in ConfigureServices() in Startup.CS file
        public RootController(IReportRepo reportRepo, IPostRepo postRepo, IUserRepo userRepo, IWebHostEnvironment webHostEnvironment, IAuthorize jwtService)
        {
            _reportRepo = reportRepo;
            _postRepo = postRepo;
            _userRepo = userRepo;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public ActionResult<ReadUserDto> Login(LoginDto dto)
        {
            if(dto.Email != "root")
                return Unauthorized();
            var user = _userRepo.GetUserByEmail(dto.Email);
            if (user == null)
                return BadRequest();

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized();
            
            //After validating all the details, create a web token and send it to the user's cookies
            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            var readUserDto = Services.MapToReadUserDto(user, _userRepo);
            return Ok(readUserDto);
        }

        [HttpGet("reports")]
        public ActionResult<IEnumerable<ReadReportDto>> GetAllReports(){
            try{
                int id = _jwtService.GetUserId(Request.Cookies["jwt"]);
                User user = _userRepo.GetUserById(id);
                if(user.Email != "root")
                    return Unauthorized();
            }catch(Exception e){
                return Unauthorized();
            }

            var postReports = _reportRepo.GetReportedPosts();
            var commentReports = _reportRepo.GetReportedComments();

            var postReportDtos = new List<ReadPostReportDto>();
            var commentReportDtos = new List<ReadCommentReportDto>();

            foreach(var report in postReports){
                if(!IncreasePostCounter(postReportDtos, report.PostId)){ // if we did not map this post - map it and add it to the list
                    postReportDtos.Add(MapToReadPostReportDto(report));
                }
            }
            
            foreach(var report in commentReports){
                if(!IncreaseCommentCounter(commentReportDtos, report.CommentId)){ // if we did not map this comment - map it and add it to the list
                    commentReportDtos.Add(MapToReadCommentReportDto(report));
                }
            }

            return Ok(new {posts = postReportDtos, comments = commentReportDtos});
        }


        /*
            This method will get a postId and list of postReportDtos
            If it's in the list - we will increase the counter of the appropriate postReport and return true.
            Else we will return false.
        */
        private bool IncreasePostCounter(IEnumerable<ReadPostReportDto> dtos, int postId){
            foreach(var dto in dtos){
                if(dto.Post.Id == postId){
                    dto.Count++;
                    return true;
                }
            }
            return false;
        }

        /*
            This method will get a comment and list of commentReportDtos
            If it's in the list - we will increase the counter of the appropriate commentReport and return true.
            Else we will return false.
        */
        private bool IncreaseCommentCounter(IEnumerable<ReadCommentReportDto> dtos, int commentId){
            foreach(var dto in dtos){
                if(dto.Comment.Id == commentId){
                    dto.Count++;
                    return true;
                }
            }
            return false;
        }

        private ReadPostReportDto MapToReadPostReportDto(PostReport report){
            return new ReadPostReportDto()
            {
                Id = report.Id,
                Count = 1,
                Post = Services.MapToReadPostDto(_postRepo.GetPostById(report.PostId),_postRepo,_userRepo)
            };
        }

        private ReadCommentReportDto MapToReadCommentReportDto(CommentReport report){
            return new ReadCommentReportDto()
            {
                Id = report.Id,
                Count = 1,
                Comment = Services.MapToReadCommentDto(_postRepo.GetCommentById(report.CommentId),_postRepo,_userRepo)
            };
        }
    }
}