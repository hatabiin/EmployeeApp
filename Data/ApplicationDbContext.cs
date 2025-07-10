using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Data
{
		public class ApplicationDbContext : DbContext
		{
			public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
					: base(options)
			{
			}
		}
}
