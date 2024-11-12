using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace UsersInfrastructure.Data.Configurations
{
    public class OperationTokenConfiguration : IEntityTypeConfiguration<OperationToken>
    {
        public void Configure(EntityTypeBuilder<OperationToken> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasConversion(
                tokenId => tokenId.Value,
                dbId => OperationTokenId.Of(dbId));

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);

            builder.Property(t => t.Code).IsRequired();
            builder.HasIndex(t => t.Code).IsUnique();

            builder.Property(t => t.OperationType)
                .HasDefaultValue(OperationType.EmailConfirmation)
                .HasConversion(
                    r => r.ToString(),
                    dbOperationType => (OperationType)Enum.Parse(typeof(OperationType), dbOperationType));

            builder.Property(t => t.Expiration).IsRequired();
        }
    }
}
