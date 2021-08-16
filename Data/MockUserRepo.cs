using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    /*
     * This class is a mock data of users
     * Basically hardcoded users data with no actual database for testing purposes
     * */
    public class MockUserRepo : IUserRepo
    {
        private readonly List<User> _users;
        private readonly List<Followers> _followers;
        public MockUserRepo()
        {
            _users = new List<User>()
            {
                new User{Id=0, Name="george", Email="georgie123@gmail.com", Password="1234"},
                new User{Id=1, Name="derek", Email="derek123@gmail.com", Password="1234"},
                new User{Id=2, Name="moses", Email="moses123@gmail.com", Password="1234"},
                new User{Id=3, Name="jesus", Email="jesus123@gmail.com", Password="1234"},
            };

            _followers = new List<Followers>()
            {
                new Followers{FollowingId=0,FollowedId=1},//george follows derek
                new Followers{FollowingId=0,FollowedId=2},//george follows moses
                new Followers{FollowingId=0,FollowedId=3},//george follows jesus

                new Followers{FollowingId=1,FollowedId=3},//derek follows jesus
                //everyone follows jesus, moses is on his own :(
            };
        }

        public bool AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(User user)
        {
            foreach (var usr in _users)
            {
                if (usr.Email == user.Email)
                    return false;
            }

            _users.Add(user);
            return true;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public IEnumerable<int> GetFollowers(int id)
        {
            List<int> followers = (from row in _followers
                                   where row.FollowedId == id
                                   select row.FollowingId).ToList();
            return followers;
        }

        public IEnumerable<int> GetFollowing(int id)
        {
            List<int> following = (from row in _followers
                                   where row.FollowingId == id
                                   select row.FollowedId).ToList();
            return following;
        }

        public IEnumerable<Message> GetMessages(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            foreach (var usr in _users)
                if (usr.Email == email)
                    return usr;
            return null;
        }

        public User GetUserById(int id)
        {
            foreach (var usr in _users)
                if (usr.Id == id)
                    return usr;
            return null;
        }
    }
}
