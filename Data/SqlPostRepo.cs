using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    /*
        This class implements the functionality of IPostRepo (The posts' repository) through SQL database
    */
    public class SqlPostRepo : IPostRepo
    {
        private readonly PostContext _context; //SQL database must have a context class

        public SqlPostRepo(PostContext context)
        {
            _context = context;
        }

        public bool AddPost(Post post)
        {
            _context.Posts.Add(post);

            return _context.SaveChanges() > 0; // save the changes to the database
        }

        /*
            Remove an existing post from the database.
            In order to do that we also need to remove all of it's likes and comments
        */
        public bool RemovePost(int postId)
        {
            var post = GetPostById(postId);
            if (post is null)
                throw new Exception("Post does not exist");
            
            //get likes and comments of the post
            var post_comments = GetPostComments(postId);
            var post_likes = (from row in _context.Likes
                                where row.PostId == postId
                                select row).ToList();
            
            //remove likes and comments of the post
            foreach(var comment in post_comments)
                _context.Comments.Remove(comment);
            foreach(var like in post_likes)
                _context.Likes.Remove(like);
            
            //remove the post
            _context.Posts.Remove(post);

            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }


        /*
            Get the list of all users' id's that likes this postId
        */
        public IEnumerable<int> GetLikes(int postId)
        {
            var likes = (from row in _context.Likes
                    where row.PostId == postId
                    select row.UserId).ToList();
            return likes;
        }

        public Post GetPostById(int postId)
        {
            var post = (from row in _context.Posts
                        where row.Id == postId
                        select row).FirstOrDefault();
            return post;
        }

        //Get all the posts this userId has uploaded
        public IEnumerable<Post> GetUserPosts(int userId)
        {
            var posts = (from row in _context.Posts
                         where row.UserId == userId
                         select row).ToList();
            return posts;
        }

        public IEnumerable<Post> Feed(int userId)
        {
            throw new NotImplementedException();
        }

        public bool LikePost(int postId, int userId)
        {
            // check that the post exists
            var post = (from row in _context.Posts
                        where row.Id == postId
                        select row).FirstOrDefault();
            if (post is null)
                throw new Exception("Post does not exist!");
            
            //check that user does not already like this post
            var like = (from row in _context.Likes
                        where row.PostId == postId && row.UserId == userId
                        select row).FirstOrDefault();
            if (like != null)
                throw new Exception("you already like this post");

            //add the like to the database
            _context.Likes.Add(new Likes { PostId = postId, UserId = userId });
            return _context.SaveChanges() > 0;
        }

        public bool UnLikePost(int postId, int userId)
        {
            // check that the post exists
            var post = (from row in _context.Posts
                        where row.Id == postId
                        select row).FirstOrDefault();
            if (post is null)
                throw new Exception("Post does not exist!");
            
            //check that user does like this post
            var like = (from row in _context.Likes
                        where row.PostId == postId && row.UserId == userId
                        select row).FirstOrDefault();

            if (like is null)
                return false;
            

            //remove the like from the database
            _context.Remove(like);
            return _context.SaveChanges() > 0;
        }

        //add a new comment to a post
        public bool Comment (int postId, Comments comment)
        {
            //check that the post exists
            var post = (from row in _context.Posts
                        where row.Id == postId
                        select row).FirstOrDefault();
            if (post is null)
                throw new Exception("Post does not exist!");

            //add the comment to the database
            _context.Comments.Add(comment);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveComment(int commentId, int userId)
        {
            var commentToRemove = (from row in _context.Comments
                        where row.Id == commentId
                        select row).FirstOrDefault();
            if (commentToRemove is null)
                throw new Exception("Comment does not exist!");
            if (commentToRemove.UserId != userId) //user must be the owner of the comment in order to remove it
                throw new Exception("Not your comment!");

            _context.Comments.Remove(commentToRemove);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Comments> GetAllComments() //get all the comments in the database from all posts
        {
            return _context.Comments.ToList();
        }


        /*
            Get all the comments of a particular post ordered by their dates from recent to old
        */
        public IEnumerable<Comments> GetPostComments(int postId)
        {
            var comments = (IEnumerable<Comments>)(from row in _context.Comments
                            where row.PostId == postId
                            select row).ToList();
            comments = comments.OrderByDescending(x => x.DatePosted);
            return comments;
        }
    }
}
