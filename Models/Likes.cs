using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Models
{
    /*
     * This class represents a table containing all the likes.
     * An example:
     *      UserId  PostId
     *      2       5
     * Here user 2 likes post 5.
     * */
    public class Likes
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
