using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent from the client when he wants to create a new post object.
    */
    public class WritePostDto
    {
        public int Id { get; set; } // post id
        public int UserId { get; set; } //post owner id
        public string ImagePath { get; set; } // picture of the post
        public string Description { get; set; } // text of the post
        public DateTime DatePosted { get; set; }
    }
}
