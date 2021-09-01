using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public class SqlPostRepo : IPostRepo
    {
        private readonly PostContext _context;

        public SqlPostRepo(PostContext context)
        {
            _context = context;
        }

        public bool AddPost(Post post)
        {
            _context.Posts.Add(post);

            return _context.SaveChanges() > 0;
        }

        public bool RemovePost(int postId)
        {
            var post = GetPostById(postId);
            if (post is null)
                throw new Exception("Post does not exist");
            _context.Remove(post);

            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

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

        public IEnumerable<Post> GetUserPosts(int userId)
        {
            var posts = (from row in _context.Posts
                         where row.UserId == userId
                         select row).ToList();
            return posts;
        }

        public bool LikePost(int postId, int userId)
        {
            var post = (from row in _context.Posts
                        where row.Id == postId
                        select row).FirstOrDefault();
            if (post is null)
                throw new Exception("Post does not exist!");

            var like = (from row in _context.Likes
                        where row.PostId == postId && row.UserId == userId
                        select row).FirstOrDefault();
            if (like != null)
                throw new Exception("you already like this post");

            _context.Likes.Add(new Likes { PostId = postId, UserId = userId });
            return _context.SaveChanges() > 0;
        }

        public bool UnLikePost(int postId, int userId)
        {
            var post = (from row in _context.Posts
                        where row.Id == postId
                        select row).FirstOrDefault();
            if (post is null)
                throw new Exception("Post does not exist!");

            var like = (from row in _context.Likes
                        where row.PostId == postId && row.UserId == userId
                        select row).FirstOrDefault();

            if (like is null)
                return false;

            _context.Remove(like);
            return _context.SaveChanges() > 0;
        }
    }
}
