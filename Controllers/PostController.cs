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
        private readonly IPostRepo _postRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JwtService _jwtService;
        private readonly IUserRepo _userRepo;

        public PostController(IPostRepo repo, IWebHostEnvironment webHostEnvironment, JwtService jwtService, IUserRepo userRepo)
        {
            _postRepo = repo;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
            _userRepo = userRepo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAllPosts()
        {
            return Ok(_postRepo.GetAllPosts());
        }

        [HttpGet("id/{postId}")]
        public ActionResult<ReadPostDto> GetPostById(int postId)
        {
            var post = _postRepo.GetPostById(postId);
            if (post is null)
                return NotFound();

            var dto = MapToReadPostDto(post);
            return Ok(dto);
        }

        [HttpGet("user/id/{userId}")]
        public ActionResult<IEnumerable<ReadPostDto>> GetUserPosts(int userId)
        {
            var posts = _postRepo.GetUserPosts(userId);
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
            _postRepo.AddPost(post);
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
                _postRepo.RemovePost(postId);
            }
            catch (Exception err) { return NotFound("Post is not found"); }

            return Ok();

        }
        
        [HttpPost("like/id/{postId}")]
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
                _postRepo.LikePost(postId, userId);
            }
            catch (Exception err) { return BadRequest(err.Message); }

            return Ok();
        }

        [HttpDelete("unlike/id/{postId}")]
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
                _postRepo.UnLikePost(postId, userId);
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
            _postRepo.Comment(postId, comment);

            return Created("Comment posted succesfully",comment);
        }

        [HttpDelete("removeComment/post/{commentId}")]
        public ActionResult RemoveComment(int commentId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                _postRepo.RemoveComment(commentId,_jwtService.GetUserId(jwt));
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
            return Ok(_postRepo.GetAllComments());
        }

        [HttpGet("feed")]
        public ActionResult GetFeed()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var myId = _jwtService.GetUserId(jwt);
                IEnumerable<Post> myPosts = _postRepo.GetUserPosts(myId);
                var following = _userRepo.GetFollowing(myId);
                IEnumerable<Post> followingPosts = new List<Post>();
                foreach (var user in following)
                {
                    followingPosts = followingPosts.Concat(_postRepo.GetUserPosts(user));
                }

                var posts = myPosts.Concat(followingPosts);
                posts = posts.OrderByDescending(x => x.DatePosted);

                var feed = new List<ReadPostDto>();
                foreach (var item in posts)
                {
                    feed.Add(MapToReadPostDto(item));
                }
                return Ok(feed);
            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            

        }
        private ReadPostDto MapToReadPostDto(Post post)
        {
            return new ReadPostDto()
            {
                Id = post.Id,
                User = Services.MapToReadUserDto(_userRepo.GetUserById(post.UserId),_userRepo),
                Description = post.Description,
                ImagePath = post.ImagePath,
                DatePosted = post.DatePosted,
                likes = _postRepo.GetLikes(post.Id)
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
