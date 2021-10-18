using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent to the client when he wants to get a user object.
    */
    public class ReadUserDto
    {
        public int Id { get; set; } // user id
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; } // user profile picture
        public List<int> Following { get; set; } // list of all the id's of the people this user is following
        public List<int> Followers { get; set; } // list of all the id's of the people following this user
    }
}
