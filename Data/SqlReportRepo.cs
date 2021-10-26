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
            return (from row in _context.Reports
                            where (row is CommentReport)
                            select row).ToList().ConvertAll(x => (CommentReport) x);
        }

        public IEnumerable<PostReport> GetReportedPosts()
        {
            return (from row in _context.Reports
                            where (row is PostReport)
                            select row).ToList().ConvertAll(x => (PostReport) x);
        }

        /*
            Remove all reports on a particular comment
        */
        public bool RemoveAllCommentReports(int commentId)
        {
            var list = (from row in _context.Reports
                        where row is CommentReport && ((CommentReport)row).CommentId == commentId
                        select row).ToList();
            if(list.Count==0)
                return true;
            _context.RemoveRange(list);
            return _context.SaveChanges() > 0;
        }

        /*
            Remove all comment reports on a particular post
        */
        public bool RemoveAllCommentReportsFromPost(int postId)
        {
            var list = (from row in _context.Reports
                        where row is CommentReport && ((CommentReport)row).PostId == postId
                        select row).ToList();
            if(list.Count==0)
                return true;
            _context.RemoveRange(list);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveAllPostReports(int postId)
        {
            var list = (from row in _context.Reports
                        where row is PostReport && ((PostReport)row).PostId == postId
                        select row).ToList();
            if(list.Count==0)
                return false;
            _context.RemoveRange(list);
            return _context.SaveChanges() > 0;
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