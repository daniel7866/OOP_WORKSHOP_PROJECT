using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    public class ReadCommentDto
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImagePath { get; set; }
        public int PostId { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }


    }
}
