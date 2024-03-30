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
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().HasKey(u => u.id);
        modelBuilder.Entity<User>().HasIndex(u => u.username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.email).IsUnique();
    }
}
