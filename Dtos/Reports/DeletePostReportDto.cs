using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Dtos
{
    /*
        This is the object the root is sending when removing a report.
        Removing a report has two options:
        Remove = true if we are removing the reported post
        Remove = false if we are only removing the report
    */
        public class DeletePostReportDto
        {
            public int PostId{get;set;}
            public bool Remove{get;set;}
        }
    }

