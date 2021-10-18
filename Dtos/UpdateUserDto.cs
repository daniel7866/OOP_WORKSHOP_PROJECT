using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent from the client when he wants to change his account information.
        If he wants to change his password he must provide his old password as well.
    */
    public class UpdateUserDto
    {

        public string Name { get; set; } // a new name
        public string Email { get; set; } // a new email
        public string OldPassword { get; set; }
        public string Password { get; set; } // a new password
        public string ImagePath { get; set; } // a new profile picture
    }
}
