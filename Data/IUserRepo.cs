using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public interface IUserRepo
    {
        bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        IEnumerable<User> GetAllUsers();

        User GetUserById(int id);

        User GetUserByEmail(string email);

        bool AddUser(User user);

        bool UpdateUserInfo(UpdateUserDto newInfo,int userId); // change details of existing user in the database

        bool FollowUser(int followingId, int followedId);

        bool UnfollowUser(int follwingId, int followedId);

        //get all users' ids that this user is following
        IEnumerable<int> GetFollowing(int userId);
        //get all users' ids that follow this user
        IEnumerable<int> GetFollowers(int userId);

        IEnumerable<User> SearchUser(String searchInput); // search for users using a search input

        bool AddMessage(Message message);

        IEnumerable<Message> GetMessages(int userId); // get all the messages of a particular user(that he sent and received)

        IEnumerable<int> GetMessagedUsers(int loggedUserId); // get all the users' id's that have messages with this user

        IEnumerable<Message> GetMessagesFromUser(int loggedUserId, int userId); // get messages involving two users
    }
}
