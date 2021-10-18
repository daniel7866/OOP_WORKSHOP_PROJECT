using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OOP_WORKSHOP_PROJECT.Models;
using OOP_WORKSHOP_PROJECT.Dtos;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent to the client when he wants to get a post object.
    */
    public class ReadPostDto
    {
        public int Id { get; set; }

        public ReadUserDto User { get; set; } // the owner of the post

        public string ImagePath { get; set; } // the image of the post

        public string Description { get; set; } // the text of the post

        public DateTime DatePosted { get; set; }

        public IEnumerable<int> likes { get; set; } // list of all the user id who likes this post

        public IEnumerable<ReadCommentDto> Comments { get; set; } // all the comments made to this post
    }
}
