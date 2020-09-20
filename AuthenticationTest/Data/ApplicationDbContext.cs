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

    }
}
