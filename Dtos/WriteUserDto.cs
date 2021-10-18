using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent from the client when he wants to create a new user.
    */
    public class WriteUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; } // profile picture
    }
}
