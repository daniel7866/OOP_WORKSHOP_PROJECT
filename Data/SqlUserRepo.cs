using Microsoft.Data.SqlClient;
using OOP_WORKSHOP_PROJECT.Dtos;
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

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateUserInfo(UpdateUserDto newInfo,int userId)
        {
            var user = (from row in _context.Users
                        where row.Id == userId
                        select row).FirstOrDefault();
            if (user == null)
                return false;

            if (!String.IsNullOrEmpty(newInfo.Name))
            {
                user.Name = newInfo.Name; 
            }


            if (!String.IsNullOrEmpty(newInfo.Email))
            {
                user.Email = newInfo.Email;
            }

            if (!String.IsNullOrEmpty(newInfo.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(newInfo.Password);
            }

            if (!String.IsNullOrEmpty(newInfo.ImagePath))
            {
                user.ImagePath = newInfo.ImagePath;
            }

                return _context.SaveChanges() > 0;

        }

        public bool FollowUser(int following, int followed)
        {
            var user = (from row in _context.Users
                        where row.Id == followed
                        select row).FirstOrDefault();
            if (user is null)
                throw new Exception("User does not exist!");

            var follow = (from row in _context.Followers
                        where row.FollowingId == following && row.FollowedId == followed
                        select row).FirstOrDefault();

            if (follow is not null)
                throw new Exception("you are already following that user");

            _context.Followers.Add(new Followers { FollowingId = following, FollowedId = followed });
            return _context.SaveChanges() > 0;
        }

        public bool UnfollowUser(int following, int followed)
        {
            var user = (from row in _context.Users
                        where row.Id == followed
                        select row).FirstOrDefault();
            if (user is null)
                throw new Exception("User does not exist!");

            var follow = (from row in _context.Followers
                          where row.FollowingId == following && row.FollowedId == followed
                          select row).FirstOrDefault();
            if (follow is null)
                throw new Exception("you aren't following that user!");
            _context.Followers.Remove(follow);
            return _context.SaveChanges() > 0;

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

        public IEnumerable<User> SearchUser(string searchInput)
        {
            var searchResults = (from row in _context.Users
                             where row.Name.Contains(searchInput)
                             select row).ToList();
            var emailResult = GetUserByEmail(searchInput);
            if (emailResult != null)
                searchResults.Insert(0, emailResult);

            return searchResults;
        }

        public IEnumerable<Message> GetMessages(int userId)
        {
            var messages = (from row in _context.Messages
                            where row.ReceiverId == userId || row.SenderId == userId
                            select row).ToList();
            return messages;
        }

        public User GetUserByEmail(string email)
        {
            var user = (from row in _context.Users
                             where row.Email == email
                             select row).FirstOrDefault();
            return user;
        }

        public User GetUserById(int userId)
        {
            var user = (from row in _context.Users
                        where row.Id == userId
                        select row).FirstOrDefault();
            return user;
        }

        public bool AddMessage(Message message)
        {
            _context.Messages.Add(message);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<int> GetMessagedUsers(int loggedUserId){
            var senders = (from row in _context.Messages
                         where row.ReceiverId == loggedUserId
                         select row.SenderId).ToList();
            var receivers = (from row in _context.Messages
                         where row.SenderId == loggedUserId
                         select row.ReceiverId).ToList();
            
            return senders.Concat(receivers).Distinct().ToList();
        }

        public IEnumerable<Message> GetMessagesFromUser(int loggedUserId, int userId){
            var messages = (from row in _context.Messages
                            where ((row.ReceiverId == userId && row.SenderId == loggedUserId) || (row.ReceiverId == loggedUserId && row.SenderId == userId))
                            select row).ToList();
            return messages;
        }
    }
}
