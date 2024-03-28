using Dynamic_Eye.Models;
using Microsoft.EntityFrameworkCore;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the table name and schema if needed
        modelBuilder.Entity<User>().ToTable("users");

        // Configure primary key
        modelBuilder.Entity<User>().HasKey(u => u.id);

        // Configure uniqueness for Username and Email
        modelBuilder.Entity<User>().HasIndex(u => u.username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.email).IsUnique();

        // Additional configuration if needed
    }
}
