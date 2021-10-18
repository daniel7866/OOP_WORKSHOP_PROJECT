using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent to the client when he wants to get a comment object.
    */
    public class ReadCommentDto
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } //the user id who created the comment
        public string UserName { get; set; } //the user name who created the comment
        public string UserImagePath { get; set; } //the user profile picture
        public int PostId { get; set; } //the post the comment is on
        public string Body { get; set; } //the comment text
        public DateTime DatePosted { get; set; }


    }
}
