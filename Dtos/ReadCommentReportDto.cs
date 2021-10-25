using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This object is sent to the client when he wants to get a comment report
    */
    public class ReadCommentReportDto : ReadReportDto
    {
        public ReadCommentDto Comment {get; set;} // the reported comment
    }
}
