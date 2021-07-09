using Microsoft.AspNetCore.Hosting;
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
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(IPostRepo repo, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost("like/post{postId}/user{userId}")]
        public ActionResult LikePost(int postId, int userId)
        {
            try
            {
                _repo.LikePost(postId, userId);
            }
            catch (Exception err) { return NotFound("Post is not found"); }

            return Ok();
        }

        private ReadPostDto MapToReadPostDto(Post post)
        {
            return new ReadPostDto()
            {
                Id = post.Id,
                UserId = post.UserId,
                Description = post.Description,
                ImagePath = post.ImagePath,
                likes = _repo.GetLikes(post.Id)
            };
        }
    }
}
