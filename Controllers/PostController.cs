using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OOP_WORKSHOP_PROJECT.Data;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OOP_WORKSHOP_PROJECT.Authorization;

namespace OOP_WORKSHOP_PROJECT.Controllers
{
    /*
    This class will handle all http requests regarding posts.
    This includes:
    viewing posts, adding and deleting them.
    likes and comments on posts.

    The route of the http requests is: "http://ip_address:port/api/post/{a path for a specific function}"
    */

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _postRepo; //the repository in which the posts are stored
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthorize _jwtService; // handles json web tokens cookies for authorizing actions
        private readonly IUserRepo _userRepo; // the repository in which the users are stored

        private readonly IReportRepo _reportRepo;

        //all the properties will be inject using dependency injection in ConfigureServices() method in Startup.CS file
        public PostController(IPostRepo repo, IWebHostEnvironment webHostEnvironment, IAuthorize jwtService, IUserRepo userRepo, IReportRepo reportRepo)
        {
            _postRepo = repo;
            _webHostEnvironment = webHostEnvironment;
            _jwtService = jwtService;
            _userRepo = userRepo;
            _reportRepo = reportRepo;
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

            posts = posts.OrderByDescending(p => p.DatePosted);

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

            try //only the user can create a post, jwt will get his token from cookies to verify his indentity
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
                var post = _postRepo.GetPostById(postId);
                if(post.UserId != userId)
                    return Unauthorized();//if user is not the owner
                _postRepo.RemovePost(postId);
            }
            catch (Exception err) { return NotFound("Post is not found"); }

            return Ok();

        }
        
        [HttpPost("like/id/{postId}")]
        public ActionResult LikePost(int postId)
        {
            int userId;
            try // verify user's indentity to make sure no one likes this post in his name
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

        [HttpPost("comment/")]
        public ActionResult CreateComment(WriteCommentsDto dto)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                dto.UserId = _jwtService.GetUserId(jwt);

            }

            catch (Exception e)
            {
                return Unauthorized();
            }

            dto.DatePosted = DateTime.Now;

            var comment = Services.MapToComment(dto);
            
            _postRepo.Comment(comment.PostId, comment);

            return Created("Comment posted succesfully",comment);
        }

        [HttpDelete("comment/{commentId}")]
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

        [HttpGet("comments/{postId}")]
        public ActionResult GetPostComments(int postId)
        {
            var comments = _postRepo.GetPostComments(postId);

            var dtos = new List<ReadCommentDto>();

            foreach(var comment in comments)
                dtos.Add(Services.MapToReadCommentDto(comment, _postRepo, _userRepo));
            return Ok(dtos);
        }

        /**
        The feed includes all user's relevant posts sorted by decending date(from recent to old)
        Relevent posts are:
        User's own posts and all of the posts of the people he follows
        **/
        [HttpGet("feed")]
        public ActionResult<IEnumerable<ReadPostDto>> GetFeed()
        {
            try
            {
                //authorize the user
                var jwt = Request.Cookies["jwt"];
                var myId = _jwtService.GetUserId(jwt);

                IEnumerable<Post> myPosts = _postRepo.GetUserPosts(myId);//get user's posts

                //get all the posts from the people he follows
                var following = _userRepo.GetFollowing(myId);
                IEnumerable<Post> followingPosts = new List<Post>();
                foreach (var user in following)
                {
                    followingPosts = followingPosts.Concat(_postRepo.GetUserPosts(user));
                }

                var posts = myPosts.Concat(followingPosts);
                posts = posts.OrderByDescending(x => x.DatePosted); // sort them all by dates(recent to old)

                // map each post to a post DTO
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

        /*
            Add a report on abusive post.
            Get the report with the postId that is being reported.
            Check for jwt and add the user id to the report as well as current time
        */
        [HttpPost("report/post")]
        public ActionResult AddPostReport(PostReport report){
            var post = _postRepo.GetPostById(report.PostId);
            if(post == null)
                return BadRequest(new {message = "Post does not exist"});
            
            try{
                int id = _jwtService.GetUserId(Request.Cookies["jwt"]);
                report.UserId = id;
            }catch(Exception e){ return Unauthorized();}

            report.DatePosted = DateTime.Now;

            if(_reportRepo.AddReport(report))
                return Created("Created Successfully",report);
            return BadRequest();
        }

        /*
            Add a report on abusive comment.
            Get the report with the commentId that is being reported.
            Check for jwt and add the user id to the report as well as current time
        */
        [HttpPost("report/comment")]
        public ActionResult AddCommentReport(CommentReport report){
            var comment = _postRepo.GetCommentById(report.CommentId);
            if(comment == null)
                return BadRequest(new {message = "Comment does not exist"});
            
            try{
                int id = _jwtService.GetUserId(Request.Cookies["jwt"]);
                report.UserId = id;
            }catch(Exception e){ return Unauthorized();}

            report.DatePosted = DateTime.Now;

            if(_reportRepo.AddReport(report))
                return Created("Created Successfully",report);
            return BadRequest();
        }

        /*
            This function takes a post and map it to a post dto.
        */
        private ReadPostDto MapToReadPostDto(Post post)
        {
            var comments = _postRepo.GetPostComments(post.Id);
            var dtos = new List<ReadCommentDto>();
            foreach(var comment in comments)
                dtos.Add(Services.MapToReadCommentDto(comment, _postRepo, _userRepo));
            
            return new ReadPostDto()
            {
                Id = post.Id,
                User = Services.MapToReadUserDto(_userRepo.GetUserById(post.UserId),_userRepo),
                Description = post.Description,
                ImagePath = post.ImagePath,
                DatePosted = post.DatePosted,
                likes = _postRepo.GetLikes(post.Id),
                Comments = dtos
            };
        }

        /*
            This function takes a post dto from the client, and map it to a post object to store in the database
        */
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
