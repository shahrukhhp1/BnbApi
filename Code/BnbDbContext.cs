using Microsoft.EntityFrameworkCore;

namespace BnbApi
{
    public class BnbDbContext : DbContext
    {
        public BnbDbContext(DbContextOptions<BnbDbContext> options)
            : base(options)
        {
        }

        public DbSet<Feature> Feature { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Stage> Stage { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<GameVersion> GameVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(u => u.UId);
                e.Property(u => u.UId).UseIdentityColumn();
            });

            modelBuilder.Entity<GameVersion>(e =>
            {
                e.ToTable("GameVersion");
                e.HasKey(g => g.Id);
                e.Property(g => g.Id).UseIdentityColumn();
            });

            modelBuilder.Entity<Feature>(e =>
            {
                e.ToTable("Feature");
                e.HasKey(f => new { f.UId, f.Title, f.Count, f.CreatedOn });
                e.HasOne(f => f.User)
                    .WithMany(u => u.Feature)
                    .HasForeignKey(f => f.UId);
            });

            modelBuilder.Entity<Stage>(e =>
            {
                e.ToTable("Stage");
                e.HasKey(s => new { s.UId, s.Stage1, s.Star, s.CreatedOn });
                e.Property(s => s.Stage1).HasColumnName("Stage");
                e.HasOne(s => s.User)
                    .WithMany(u => u.Stage)
                    .HasForeignKey(s => s.UId);
            });

            modelBuilder.Entity<Report>(e =>
            {
                e.ToTable("Report");
                e.HasKey(r => new { r.Guid, r.Message, r.UId });
            });
        }
    }
}
