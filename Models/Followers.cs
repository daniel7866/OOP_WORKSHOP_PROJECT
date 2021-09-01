using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace OOP_WORKSHOP_PROJECT.Models
{
    /*
     * This table will represent all the people following all other people
     * For example:
     * If user_id 13 follows user_id 100, we will have the following info in the table:
     * FollowingId  FollowedId
     *      13          100
     * */
    public class Followers
    {
        [Key]
        public int Id { get; set; }
        public int FollowingId { get; set; }
        public int FollowedId { get; set; }
    }
}
