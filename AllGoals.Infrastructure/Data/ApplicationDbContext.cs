using AllGoals.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllGoals.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<StoreItem> StoreItems => Set<StoreItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(b =>
        {
            b.OwnsOne(u => u.Email, eo =>
            {
                eo.Property(e => e.Value).HasColumnName("Email").IsRequired();
            });
            b.Property(u => u.IsAdmin)
                .HasColumnType("NUMBER(1)"); 
        });
    }

}
