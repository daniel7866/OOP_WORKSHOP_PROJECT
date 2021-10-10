using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OOP_WORKSHOP_PROJECT.Models;
using OOP_WORKSHOP_PROJECT.Dtos;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    public class ReadPostDto
    {
        public int Id { get; set; }

        public ReadUserDto User { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public DateTime DatePosted { get; set; }

        public IEnumerable<int> likes { get; set; }

        public IEnumerable<ReadCommentDto> Comments { get; set; }
    }
}
