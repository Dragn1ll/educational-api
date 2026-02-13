using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleService.Domain.Entities;

namespace SimpleService.Infrastructure.PostgreSql.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasKey(up => up.Id);
        
        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfileEntity>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(up => up.UserId)
            .IsUnique();
    }
}