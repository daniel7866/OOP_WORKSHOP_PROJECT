using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Models
{
    /*
     * This class represents a post that users will upload.
     * A post is simply a picture with an optional description.
     * A post contains the post's id(Id) and the owner's id(UserId)
     * */
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string ImagePath { get; set; }

        public string Description { get; set; }
    }
}
