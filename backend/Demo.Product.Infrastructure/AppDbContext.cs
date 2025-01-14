

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

        modelBuilder.Entity<User>()
          .HasIndex(u => u.Username)
          .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity(j => j.ToTable("UserRoles"));



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
