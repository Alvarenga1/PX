using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthenticationTest.Models;

namespace AuthenticationTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<AuthenticationTest.Models.Competition> Competition { get; set; }
        public DbSet<AuthenticationTest.Models.Team> Team { get; set; }

        public DbSet<AuthenticationTest.Models.Staff> Staff { get; set; }

        public DbSet<AuthenticationTest.Models.Student> Student { get; set; }

        public DbSet<AuthenticationTest.Models.Invite> Invite { get; set; }
        public DbSet<AuthenticationTest.Models.Post> Post { get; set; }
        public DbSet<AuthenticationTest.Models.Comment> Comment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<Competition>().ToTable("Competition");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Invite>().ToTable("Invite");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Comment>().ToTable("Comment");
        }

    }
}
