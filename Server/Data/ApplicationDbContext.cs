using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Link> Links { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Link>(e =>
        {
            e.HasKey(x => x.Id);

            e.HasIndex(x => x.ShortCode)
             .IsUnique();

            e.Property(x => x.ShortCode)
             .IsRequired()
             .HasMaxLength(50);

            e.Property(x => x.TargetUrl)
             .IsRequired();
        });
    }
}
