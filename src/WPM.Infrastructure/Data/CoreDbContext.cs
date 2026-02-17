using Microsoft.EntityFrameworkCore;
using WPM.Core.Models;

namespace WPM.Infrastructure.Data;

public class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) { }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<SiteDomain> SiteDomains => Set<SiteDomain>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Site>(e =>
        {
            e.ToTable("Sites");
            e.HasKey(s => s.Id);
            e.Property(s => s.SiteName).IsRequired().HasMaxLength(200);
            e.Property(s => s.FolderName).IsRequired().HasMaxLength(200);
            e.HasIndex(s => s.FolderName).IsUnique();
        });

        modelBuilder.Entity<SiteDomain>(e =>
        {
            e.ToTable("SiteDomains");
            e.HasKey(d => d.Id);
            e.Property(d => d.Domain).IsRequired().HasMaxLength(253);
            e.HasIndex(d => d.Domain).IsUnique();
            e.HasOne(d => d.Site)
                .WithMany(s => s.Domains)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
