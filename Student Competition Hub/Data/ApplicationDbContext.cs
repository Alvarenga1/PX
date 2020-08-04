using Microsoft.EntityFrameworkCore;
using Student_Competition_Hub.Models;

namespace BadgerysCreekHotel.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Student_Competition_Hub.Models.Staff> Room { get; set; }
	}
}
