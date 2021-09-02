using Microsoft.EntityFrameworkCore;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
/*            modelBuilder.Entity<Likes>().HasNoKey();*/
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Likes> Likes { get; set; }

        public DbSet<Comments> Comments { get; set; }
    }
}
