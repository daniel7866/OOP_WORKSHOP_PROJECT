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

        //get all users' ids that this user is following
        IEnumerable<int> GetFollowing(int userId);
        //get all users' ids that follow this user
        IEnumerable<int> GetFollowers(int userId);
    }
}
