using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleService.Domain.Entities;

namespace SimpleService.Infrastructure.PostgreSql.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        builder.HasOne(u => u.Gender)
            .WithMany(a => a.Users)
            .HasForeignKey(a => a.GenderId);
        
        builder.HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfileEntity>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}