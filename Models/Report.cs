using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Models
{
    /*
     * This class represents a report on abusive content.
     * A report has:
        *It's id
        *user id that made the report
        *date reported
     * */
    public abstract class Report
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
