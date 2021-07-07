using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
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
        public ActionResult<Post> GetPostById(int postId)
        {
            var post = _repo.GetPostById(postId);
            if (post is null)
                return NotFound();
            return Ok(post);
        }

        [HttpGet("user/id/{userId}")]
        public ActionResult<IEnumerable<Post>> GetUserPosts(int userId)
        {
            var posts = _repo.GetUserPosts(userId);
            if (posts is null)
                return NotFound();
            return Ok(posts);
        }
    }
}
