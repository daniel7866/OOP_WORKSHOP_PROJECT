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

        IEnumerable<Post> GetAllPosts();

        Post GetPostById(int postId);

        IEnumerable<Post> GetUserPosts(int userId);

        bool AddPost(Post post);

        //get all users' ids that likes this postId
        IEnumerable<int> GetLikes(int postId);

        bool LikePost(int postId, int userId);

        bool UnLikePost(int postId, int userId);
    }
}
