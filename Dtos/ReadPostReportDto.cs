using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent to the client when he wants to get a post report
    */
    public class ReadPostReportDto : ReadReportDto
    {
        public ReadPostDto Post {get; set;} // the reported post
    }
}
