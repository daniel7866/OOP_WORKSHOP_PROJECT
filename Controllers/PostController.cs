using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OOP_WORKSHOP_PROJECT.Helpers;

namespace OOP_WORKSHOP_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JwtService _jwtService;

        public PostController(IPostRepo repo, IWebHostEnvironment webHostEnvironment, JwtService jwtService)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAllPosts()
        {
            return Ok(_repo.GetAllPosts());
        }

        [HttpGet("id/{postId}")]
        public ActionResult<ReadPostDto> GetPostById(int postId)
        {
            var post = _repo.GetPostById(postId);
            if (post is null)
                return NotFound();

            var dto = MapToReadPostDto(post);
            return Ok(dto);
        }

        [HttpGet("user/id/{userId}")]
        public ActionResult<IEnumerable<ReadPostDto>> GetUserPosts(int userId)
        {
            var posts = _repo.GetUserPosts(userId);
            if (posts is null)
                return NotFound();

            var dtos = new List<ReadPostDto>();
            foreach (var item in posts)
            {
                dtos.Add(MapToReadPostDto(item));
            }
            return Ok(dtos);
        }

        [HttpPost("CreatePost")]
        public ActionResult CreatePost(WritePostDto dto)
        {
            dto.DatePosted = DateTime.Now;

            try
            {
                var jwt = Request.Cookies["jwt"];
                dto.UserId = _jwtService.GetUserId(jwt);
            }
            catch (Exception e) { return Unauthorized(); }
            Post post = MapToPost(dto);
            _repo.AddPost(post);
            return Created("success", post);

        }

        [HttpDelete("DeletePost/{postId}")]
        public ActionResult DeletePost(int postId)
        {
            int userId;
            try
            {
                var jwt = Request.Cookies["jwt"];
                userId = _jwtService.GetUserId(jwt);

            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            try
            {
                _repo.RemovePost(postId);
            }
            catch (Exception err) { return NotFound("Post is not found"); }

            return Ok();

        }
        
        [HttpPost("like/post/{postId}")]
        public ActionResult LikePost(int postId)
        {
            int userId;
            try
            {
                var jwt = Request.Cookies["jwt"];
                userId = _jwtService.GetUserId(jwt);

            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            try
            {
                _repo.LikePost(postId, userId);
            }
            catch (Exception err) { return BadRequest(err.Message); }

            return Ok();
        }

        [HttpDelete("unlike/post/{postId}")]
        public ActionResult UnlikePost(int postId)
        {
            int userId;
            try
            {
                var jwt = Request.Cookies["jwt"];
                userId = _jwtService.GetUserId(jwt);

            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            try
            {
                _repo.UnLikePost(postId, userId);
            }
            catch (Exception err) { return BadRequest(err.Message); }

            return Ok();
        }

        [HttpPost("comment/post/{postId}")]
        public ActionResult CreateComment(Comments comment, int postId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                comment.UserId = _jwtService.GetUserId(jwt);

            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            comment.PostId = postId;
            _repo.Comment(postId, comment);

            return Created("Comment posted succesfully",comment);
        }

        [HttpDelete("removeComment/post/{commentId}")]
        public ActionResult RemoveComment(int commentId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                _repo.RemoveComment(commentId,_jwtService.GetUserId(jwt));
            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            return Ok("Comment Removed");
        }


        [HttpGet("comments")]
        public ActionResult GetAllComments()
        {
            return Ok(_repo.GetAllComments());
        }

        private ReadPostDto MapToReadPostDto(Post post)
        {
            return new ReadPostDto()
            {
                Id = post.Id,
                UserId = post.UserId,
                Description = post.Description,
                ImagePath = post.ImagePath,
                DatePosted = post.DatePosted,
                likes = _repo.GetLikes(post.Id)
            };
        }

        private Post MapToPost(WritePostDto dto)
        {

            return new Post()
            {
                UserId = dto.UserId,
                ImagePath = dto.ImagePath,
                Description = dto.Description,
                DatePosted = dto.DatePosted
            };
        }
    }
}
