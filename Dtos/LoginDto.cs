using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This is the object the client is sending when trying to log in
    */
        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { set; get; }
        }
    }

