using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    /*
        This class implements the functionality of IPostRepo (The posts' repository) through SQL database
    */
    public class SqlReportRepo : IReportRepo
    {
        private readonly PostContext _context; //SQL database must have a context class

        public SqlReportRepo(PostContext context)
        {
            _context = context;
        }

        public bool AddReport(Report report)
        {
            _context.Reports.Add(report);

            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _context.Reports;
        }

        public Report GetReportById(int id)
        {
            return (from row in _context.Reports
                            where row.Id == id
                            select row).FirstOrDefault();
        }

        public IEnumerable<CommentReport> GetReportedComments()
        {
            return (IEnumerable<CommentReport>) (from row in _context.Reports
                            where (row is CommentReport)
                            select row).ToList();
        }

        public IEnumerable<PostReport> GetReportedPosts()
        {
            return (IEnumerable<PostReport>) (from row in _context.Reports
                            where (row is PostReport)
                            select row).ToList();
        }

        public bool RemoveReport(int id)
        {
            var report = (from row in _context.Reports
                            where row.Id == id
                            select row).FirstOrDefault();
            
            _context.Remove(report);

            return _context.SaveChanges() > 0;
        }
    }
}