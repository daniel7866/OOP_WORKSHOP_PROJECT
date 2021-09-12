using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public class MockPostRepo : IPostRepo
    {
        private readonly List<Post> _posts;
        private readonly List<Likes> _likes;
        public MockPostRepo()
        {
            _posts = new List<Post>()
            {
                new Post
                {
                    Id=0,
                    UserId=3,
                    ImagePath="C:\\Users\\danie\\source\\repos\\OOP_WORKSHOP_PROJECT\\wwwroot\\uploads\\moon.JPG",
                    Description="This is a picture of the moon"
                }//jesus uploads a post
            };

            _likes = new List<Likes>()
            {
                new Likes{UserId=0,PostId=0}//george likes the post
            };
        }
        public bool AddPost(Post post)
        {
            _posts.Add(post);
            return true;
        }

        public bool RemovePost(int postId)
        {
            var post = GetPostById(postId);
            if (post is null)
                throw new Exception("Post does not exist");

            _posts.Remove(post);

            return true;


        }
        

        public IEnumerable<Post> GetAllPosts()
        {
            return _posts;
        }

        public IEnumerable<int> GetLikes(int postId)
        {
            List<int> users = (from row in _likes
                               where row.PostId == postId
                               select row.UserId).ToList();
            return users;
        }

        public Post GetPostById(int postId)
        {
            var post = (from row in _posts
                       where row.Id == postId
                       select row).FirstOrDefault();
            return (Post)post;
        }

        public IEnumerable<int> GetPostLikes(int postId)
        {
            var post = GetPostById(postId);
            if (post is null)
                throw new Exception("Post does not exist");

            var liked_user_id = (from row in _likes
                         where row.PostId == postId
                         select row.UserId).ToList();
            return liked_user_id;
        }

        public IEnumerable<Post> GetUserPosts(int userId)
        {
            var posts = from row in _posts
                       where row.UserId == userId
                       select row;
            return posts;
        }

        public bool LikePost(int postId, int userId)
        {
            var post = GetPostById(postId);
            if (post is null)
                throw new Exception("Post does not exist");
            var like = (from row in _likes
                       where row.PostId == postId && row.UserId == userId
                       select row).FirstOrDefault();
            if (like is not null)
                return false;
            _likes.Add(new Likes() { PostId = postId, UserId = userId });
            return true;
        }

        public bool UnLikePost(int postId, int userId)
        {
            var post = GetPostById(postId);
            if (post is null)
                throw new Exception("Post does not exist");
            var like = (from row in _likes
                       where row.PostId == postId && row.UserId == userId
                       select row).FirstOrDefault();
            if (like is null)
                return false;

            _likes.Remove(like);
            return true;
        }

        public bool Comment(int postId, Comments comment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comments> GetAllComments()
        {
            throw new NotImplementedException();
        }

        public bool RemoveComment()
        {
            throw new NotImplementedException();
        }

        public bool RemoveComment(int commentId, int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> Feed(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
