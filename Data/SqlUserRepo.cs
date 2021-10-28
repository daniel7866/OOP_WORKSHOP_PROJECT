using Microsoft.Data.SqlClient;
using OOP_WORKSHOP_PROJECT.Dtos;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    /*
        This class implements the functionality of IUserRepo (The users' repository) through SQL database
    */
    public class SqlUserRepo : IUserRepo
    {
        private readonly UserContext _context; //context to connect to the databse

        public SqlUserRepo(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers() // get all the users in the system
        {
            return _context.Users.ToList();
        }

        /*
            Register a new user to the database
        */
        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }

        /*
            Update details of existing user in the database
        */
        public bool UpdateUserInfo(UpdateUserDto newInfo,int userId)
        {
            //check that the user does exist
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
                user.Password = BCrypt.Net.BCrypt.HashPassword(newInfo.Password); // all passwords are hashed in the database
            }

            if (!String.IsNullOrEmpty(newInfo.ImagePath))
            {
                user.ImagePath = newInfo.ImagePath;
            }

                return _context.SaveChanges() > 0;

        }

        public bool FollowUser(int following, int followed)
        {
            //check that user exist
            var user = (from row in _context.Users
                        where row.Id == followed
                        select row).FirstOrDefault();
            if (user is null)
                throw new Exception("User does not exist!");

            //check that you're not already following this user
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
            //check that user exist
            var user = (from row in _context.Users
                        where row.Id == followed
                        select row).FirstOrDefault();
            if (user is null)
                throw new Exception("User does not exist!");
            
            //check that you're already following this user
            var follow = (from row in _context.Followers
                          where row.FollowingId == following && row.FollowedId == followed
                          select row).FirstOrDefault();
            if (follow is null)
                throw new Exception("you aren't following that user!");
            
            _context.Followers.Remove(follow);
            return _context.SaveChanges() > 0;

        }

        /*
            Get the list of id's of all users following this particular user
        */
        public IEnumerable<int> GetFollowers(int userId)
        {
            var followers = (from row in _context.Followers
                             where row.FollowedId == userId
                             select row.FollowingId).ToList();
            return followers;
        }

        /*
            Get the list of id's of all users this particular user is following
        */
        public IEnumerable<int> GetFollowing(int userId)
        {
            var following = (from row in _context.Followers
                             where row.FollowingId == userId
                             select row.FollowedId).ToList();
            return following;
        }

        /*
            Search for users using a search input.

            Users can be searched by name:
                -can be a partial name and return all matching users
            
            Users can be searched by email:
                -email must be exact and returns one matching user(if there is one)
        */
        public IEnumerable<User> SearchUser(string searchInput)
        {
            //search for matching names
            var searchResults = (from row in _context.Users
                             where row.Name.Contains(searchInput)
                             select row).ToList();
            
            //search for matching email
            var emailResult = GetUserByEmail(searchInput);
            if (emailResult != null)
                searchResults.Insert(0, emailResult);

            return searchResults;
        }

        /*
            Get all the messages involving this user (messages he sent and received)
        */
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

        /*
            Get a list of all users' id's that sent or received messages from this userId
        */
        public IEnumerable<int> GetMessagedUsers(int loggedUserId){
            //users that sent this user messages
            var senders = (from row in _context.Messages
                         where row.ReceiverId == loggedUserId
                         select row.SenderId).ToList();
            
            //users that receieved messages from this user
            var receivers = (from row in _context.Messages
                         where row.SenderId == loggedUserId
                         select row.ReceiverId).ToList();
            
            return senders.Concat(receivers).Distinct().ToList();
        }

        /*
            Get all the messages that involving these two users
        */
        public IEnumerable<Message> GetMessagesFromUser(int loggedUserId, int userId){
            var messages = (from row in _context.Messages
                            where ((row.ReceiverId == userId && row.SenderId == loggedUserId) || (row.ReceiverId == loggedUserId && row.SenderId == userId))
                            select row).ToList();
            return messages;
        }

        /*
            Get all the userId that sent messages to 'receiverId' that has not been read yet
        */
        public IEnumerable<int> GetUnreadMessagedUsers(int receiverId)
        {
            return (from row in _context.UnreadMessagedUsers
                                        where row.ReceiverId == receiverId
                                        select row.SenderId).ToList();
        }

        /*
            mark as read - remove user from the unreadmessage table
        */
        public bool RemoveUnreadMessagedUser(int receiverId, int senederId)
        {
            var entry = (from row in _context.UnreadMessagedUsers
                         where row.ReceiverId == receiverId && row.SenderId == senederId
                         select row).ToList();
            
            _context.RemoveRange(entry);

            return _context.SaveChanges() > 0;
        }

        /*
            mark as unread - add user to the unreadMessage table
        */
        public bool AddUnreadMessagedUser(int receiverId, int senderId)
        {
            UnreadMessagedUsers entry = new UnreadMessagedUsers(){
                ReceiverId = receiverId,
                SenderId = senderId
            };

            _context.Add(entry);

            return _context.SaveChanges() > 0;
        }
    }
}
