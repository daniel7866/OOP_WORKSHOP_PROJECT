using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    public class ReadMessageDto
    {
        [Key]
        public int Id
        {
            get; set;
        }
        public int UserId
        {
            get; set;
        }
        public int PostId
        {
            get; set;
        }
        public string Body
        {
            get; set;
        }
        public DateTime DatePosted
        {
            get; set;
        }
    }
}
