using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent from the client when he wants to create a new message object.
    */
    public class WriteMessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
        public DateTime DateSent { get; set; }
    }
}
