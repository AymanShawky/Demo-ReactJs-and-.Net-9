

using Demo.Product.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Demo.Product.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }


    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Entities.UserRole> UserRoles { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Entities.Product> Products { get; set; }

    public DbSet<Entities.ProductRating> Ratings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
             
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(item =>
        {
            item.HasKey(u => u.Id);
            item.Property(u => u.Id)
                .UseIdentityColumn(seed: 1, increment: 1);

            item.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            item.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(250);

            item.ToTable("Users");
        });

        modelBuilder.Entity<Role>(item =>
        {
            item.HasKey(u => u.Id);
            item.Property(u => u.Id)
                .UseIdentityColumn(seed: 1, increment: 1);

            item.Property(u => u.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            item.Property(u => u.RoleDescription)
                .HasMaxLength(250);

            item.ToTable("Roles");
        });

        modelBuilder.Entity<Role>()
       .HasIndex(r => r.RoleName)
       .IsUnique();

        modelBuilder.Entity<User>()
          .HasIndex(u => u.Username)
          .IsUnique();

        modelBuilder.Entity<User>()
         .HasMany(u => u.Roles)
         .WithMany(r => r.Users)
         .UsingEntity<UserRole>(
             j => j
                 .HasOne(ur => ur.Role)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.RoleId),
             j => j
                 .HasOne(ur => ur.User)
                 .WithMany(u => u.UserRoles)
                 .HasForeignKey(ur => ur.UserId),
             j =>
             {
                 j.HasKey(ur => new { ur.UserId, ur.RoleId });
                 j.ToTable("UserRoles");
             });


        modelBuilder.Entity<RefreshToken>(item =>
        {
            item.HasKey(u => u.Id);
            item.Property(u => u.Id)
                .UseIdentityColumn(seed: 1, increment: 1);

            item.Property(u => u.Token)
                .IsRequired()
                .HasMaxLength(50);

            item.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            item.ToTable("RefreshTokens");
        });

        modelBuilder.Entity<Infrastructure.Entities.Product>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ProductRating>()
            .ToTable("ProductRatings");

        // comment this code because every migration generate new records for the hash
        //// data seeding
        //modelBuilder.Entity<User>()
        //    .HasData(
        //        new User { Id = 1, Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("test") },
        //        new User { Id = 2, Username = "user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("test") }
        //    );

        //modelBuilder.Entity<Role>()
        //    .HasData(
        //        new Role { Id = 1, RoleName = "Admin", RoleDescription = "Admin role" },
        //        new Role { Id = 2, RoleName = "User", RoleDescription = "User role" }
        //    );
    }
}
