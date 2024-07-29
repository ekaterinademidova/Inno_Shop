﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace UsersInfrastucture.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasConversion(
                userId => userId.Value,
                dbId => UserId.Of(dbId));

            builder.HasMany(u => u.CreatedProducts)
                .WithOne()
                .HasForeignKey(p => p.CreatedByUserId);

            builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Password).HasMaxLength(100).IsRequired();

            builder.Property(u => u.Role)
                .HasDefaultValue(UserRole.User)
                .HasConversion(
                    r => r.ToString(),
                    dbRole => (UserRole)Enum.Parse(typeof(UserRole), dbRole));
            builder.Property(u => u.Status)
                .HasDefaultValue(UserStatus.Inactive)
                .HasConversion(
                    s => s.ToString(),
                    dbRole => (UserStatus)Enum.Parse(typeof(UserStatus), dbRole));
        }
    }
}
