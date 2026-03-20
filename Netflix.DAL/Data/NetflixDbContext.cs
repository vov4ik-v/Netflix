using Microsoft.EntityFrameworkCore;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Data;

public class NetflixDbContext(DbContextOptions<NetflixDbContext> options) : DbContext(options)
{
    public DbSet<Content> Contents { get; set; } = null!;
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Series> Series { get; set; } = null!;
    public DbSet<Episode> Episodes { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;
    public DbSet<MyList> MyLists { get; set; } = null!;
    public DbSet<MyListItem> MyListItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasDiscriminator<string>("ContentType")
                .HasValue<Movie>("Movie")
                .HasValue<Series>("Series");
            entity.HasOne(e => e.Genre)
                .WithMany(g => g.Contents)
                .HasForeignKey(e => e.GenreId);
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Series)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeriesId);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Content)
                .WithMany(c => c.Reviews)
                .HasForeignKey(e => e.ContentId);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Content)
                .WithMany(c => c.Ratings)
                .HasForeignKey(e => e.ContentId);
        });

        modelBuilder.Entity<MyList>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithOne(u => u.MyList)
                .HasForeignKey<MyList>(e => e.UserId);
        });

        modelBuilder.Entity<MyListItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.MyList)
                .WithMany(m => m.Items)
                .HasForeignKey(e => e.MyListId);
            entity.HasOne(e => e.Content)
                .WithMany()
                .HasForeignKey(e => e.ContentId);
        });
    }
}