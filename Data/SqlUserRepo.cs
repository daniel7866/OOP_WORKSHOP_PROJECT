using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private readonly UserContext _context;

        public SqlUserRepo(UserContext context)
        {
            _context = context;
        }

        public bool AddUser(User user)
        {
            _context.Users.Add(user);

            return _context.SaveChanges() > 0;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<int> GetFollowers(int userId)
        {
            var followers = (from row in _context.Followers
                             where row.FollowedId == userId
                             select row.FollowingId).ToList();
            return followers;
        }

        public IEnumerable<int> GetFollowing(int userId)
        {
            var following = (from row in _context.Followers
                             where row.FollowingId == userId
                             select row.FollowedId).ToList();
            return following;
        }

        public User GetUserByEmail(string email)
        {
            var following = (from row in _context.Users
                             where row.Email == email
                             select row).FirstOrDefault();
            return following;
        }

        public User GetUserById(int userId)
        {
            var user = (from row in _context.Users
                        where row.Id == userId
                        select row).FirstOrDefault();
            return user;
        }
    }
}
