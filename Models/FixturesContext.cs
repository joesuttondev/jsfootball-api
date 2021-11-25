using Microsoft.EntityFrameworkCore;

namespace jsfootball_api.Models
{
    public class FixturesContext : DbContext
    {
        public FixturesContext(DbContextOptions<FixturesContext> options)
            : base(options)
        {
        }

        public DbSet<Fixture> Fixtures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasDefaultContainer("Fixtures");
			builder.Entity<Fixture>().HasPartitionKey(f => f.id)
				.HasNoDiscriminator();
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .LogTo(Console.WriteLine)
        .EnableDetailedErrors();
    } 
}