using Microsoft.EntityFrameworkCore;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    /*
        This class connects to the post's database.
        It stores all the tables that are related to posts:
            *Posts
            *Likes to posts
            *Comments to posts
    */
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentReport>();
            modelBuilder.Entity<PostReport>();

            //Convert the 'DateTime' C# type to 'datetime2' SQL type
            modelBuilder.Entity<Post>().Property(u => u.DatePosted).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Comments>().Property(u => u.DatePosted).HasColumnType("datetime2").HasPrecision(0);
            modelBuilder.Entity<Report>().Property(u => u.DatePosted).HasColumnType("datetime2").HasPrecision(0);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Likes> Likes { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public DbSet<Report> Reports{get;set;}
    }
}
