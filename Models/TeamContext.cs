using Microsoft.EntityFrameworkCore;

namespace jsfootball_api.Models
{
    public class TeamsContext : DbContext
    {
        public TeamsContext(DbContextOptions<TeamsContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasDefaultContainer("Teams");
			builder.Entity<Team>().HasPartitionKey(t => t.id)
				.HasNoDiscriminator();
		}
    } 
}