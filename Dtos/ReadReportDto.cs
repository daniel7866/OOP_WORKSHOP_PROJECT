using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent to the client when he wants to get a report
    */
    public class ReadReportDto
    {
        public int Id { get; set; } // report id

        public int Count {get; set;} // how many reports on this post
    }
}
