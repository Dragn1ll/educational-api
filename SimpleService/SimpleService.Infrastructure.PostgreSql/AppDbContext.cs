using Microsoft.EntityFrameworkCore;
using SimpleService.Domain.Entities;
using SimpleService.Infrastructure.PostgreSql.Configurations;

namespace SimpleService.Infrastructure.PostgreSql;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<GenderEntity> Genders { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}