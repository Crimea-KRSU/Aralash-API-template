namespace Aralash.Infrastructure.Configurations;

public class SecuredOperationsConfiguration : IEntityTypeConfiguration<SecuredOperation>
{
    public void Configure(EntityTypeBuilder<SecuredOperation> builder)
    {
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.OperationName).IsRequired();
        builder
            .HasIndex(sp => sp.OperationName)
            .HasDatabaseName("ix_operation_name")
            .IsUnique();

    }
}