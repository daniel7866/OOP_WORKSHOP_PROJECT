using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public interface IReportRepo
    {
        public IEnumerable<Report> GetAllReports();

        public Report GetReportById(int id);

        public IEnumerable<PostReport> GetReportedPosts();

        public IEnumerable<CommentReport> GetReportedComments();

        public bool AddReport(Report report);

        public bool RemoveReport(int id);
    }
}