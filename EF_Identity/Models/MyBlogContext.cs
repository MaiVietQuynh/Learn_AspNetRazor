using Microsoft.EntityFrameworkCore;

namespace EF_Identity.Models
{
	public class MyBlogContext : DbContext
	{

		public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
		{

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
		public DbSet<Article> Articles { get; set; }
	}
}
