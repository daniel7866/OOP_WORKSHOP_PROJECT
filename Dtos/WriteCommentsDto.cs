using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent from the client when he wants to create a new comment object.
    */
    public class WriteCommentsDto
    {
        [Key]
        public int Id { get; set; } // comment id
        public int UserId { get; set; } // owner of the comment
        public int PostId { get; set; } // the post this comment is about
        public string Body { get; set; } // the text of the comment
        public DateTime DatePosted { get; set; }


    }
}
