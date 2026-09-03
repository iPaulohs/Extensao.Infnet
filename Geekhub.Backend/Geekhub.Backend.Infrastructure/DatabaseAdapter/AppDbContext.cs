using Geekhub.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Geekhub.Backend.Infrastructure.DatabaseAdapter;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
