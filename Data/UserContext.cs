using Microsoft.EntityFrameworkCore;
using OOP_WORKSHOP_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT.Data
{
    /*
        This class connects to the users' database.
        It stores all the tables that are related to users:
            *Users
            *Followers of users
            *Messages of users
    */
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //make sure each email in the users' table is unique
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            
            //Convert the 'DateTime' C# type to 'datetime2' SQL type
            modelBuilder.Entity<Message>().Property(u => u.DateSent).HasColumnType("datetime2").HasPrecision(0);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Followers> Followers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<UnreadMessagedUsers> UnreadMessagedUsers {get; set;}
    }
}
