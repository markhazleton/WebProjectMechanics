using Microsoft.EntityFrameworkCore;
using WPM.Domain.CMS.Models;

namespace WPM.Domain.CMS.Data;

public class CmsDbContext : DbContext
{
    private readonly string? _dbPath;

    public CmsDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }

    public CmsDbContext(DbContextOptions<CmsDbContext> options) : base(options) { }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Parameter> Parameters => Set<Parameter>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<LocationImage> LocationImages => Set<LocationImage>();
    public DbSet<LocationAlias> LocationAliases => Set<LocationAlias>();
    public DbSet<LocationGroup> LocationGroups => Set<LocationGroup>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured && _dbPath is not null)
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(e =>
        {
            e.ToTable("Locations");
            e.HasKey(l => l.Id);
            e.Property(l => l.Title).IsRequired().HasMaxLength(500);
            e.Property(l => l.Slug).IsRequired().HasMaxLength(500);
            e.HasIndex(l => l.Slug);
            e.HasOne(l => l.Parent)
                .WithMany(l => l.Children)
                .HasForeignKey(l => l.ParentLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Article>(e =>
        {
            e.ToTable("Articles");
            e.HasKey(a => a.Id);
            e.Property(a => a.Title).IsRequired().HasMaxLength(500);
            e.HasOne(a => a.Location)
                .WithMany(l => l.Articles)
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Part>(e =>
        {
            e.ToTable("Parts");
            e.HasKey(p => p.Id);
            e.Property(p => p.PartName).IsRequired().HasMaxLength(200);
            e.HasIndex(p => p.PartName);
        });

        modelBuilder.Entity<Parameter>(e =>
        {
            e.ToTable("Parameters");
            e.HasKey(p => p.Id);
            e.Property(p => p.Key).IsRequired().HasMaxLength(200);
            e.HasIndex(p => p.Key);
        });

        modelBuilder.Entity<Image>(e =>
        {
            e.ToTable("Images");
            e.HasKey(i => i.Id);
            e.Property(i => i.FileName).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<LocationImage>(e =>
        {
            e.ToTable("LocationImages");
            e.HasKey(li => li.Id);
            e.HasOne(li => li.Location)
                .WithMany(l => l.LocationImages)
                .HasForeignKey(li => li.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(li => li.Image)
                .WithMany(i => i.LocationImages)
                .HasForeignKey(li => li.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LocationAlias>(e =>
        {
            e.ToTable("LocationAliases");
            e.HasKey(a => a.Id);
            e.Property(a => a.AliasPath).IsRequired().HasMaxLength(1000);
            e.HasIndex(a => a.AliasPath);
            e.HasOne(a => a.Location)
                .WithMany(l => l.Aliases)
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LocationGroup>(e =>
        {
            e.ToTable("LocationGroups");
            e.HasKey(g => g.Id);
            e.Property(g => g.GroupName).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.HasKey(c => c.Id);
            e.Property(c => c.CategoryName).IsRequired().HasMaxLength(200);
        });
    }
}
