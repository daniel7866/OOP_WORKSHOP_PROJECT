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

        IEnumerable<Post> GetAllPosts();

        IEnumerable<Post> GetUserPosts(int userId);

        IEnumerable<Post> Feed(int userId);

        bool AddPost(Post post);

        public bool RemovePost(int postId);

        //get all users' ids that likes this postId
        IEnumerable<int> GetLikes(int postId);

        bool LikePost(int postId, int userId);

        bool UnLikePost(int postId, int userId);

        public bool Comment(int postId, Comments comment);

        public IEnumerable<Comments> GetAllComments();

        public bool RemoveComment(int commentId, int userId);
    }
}
