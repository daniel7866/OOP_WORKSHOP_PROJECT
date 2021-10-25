using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public interface IPostRepo
    {
        bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        Post GetPostById(int postId);

        Comments GetCommentById(int commentId);

        IEnumerable<Post> GetAllPosts(); //get all the posts in the database from all users

        IEnumerable<Post> GetUserPosts(int userId);

        IEnumerable<Post> Feed(int userId); //get the feed of a particular user

        bool AddPost(Post post);

        public bool RemovePost(int postId);

        //get all users' ids that likes this postId
        IEnumerable<int> GetLikes(int postId);

        bool LikePost(int postId, int userId); // userId likes postId

        bool UnLikePost(int postId, int userId);

        public bool Comment(int postId, Comments comment);

        public IEnumerable<Comments> GetAllComments(); // get all comments in the database from all users

        public IEnumerable<Comments> GetPostComments(int postId); // get all comments of a particular post

        public bool RemoveComment(int commentId, int userId);
    }
}
