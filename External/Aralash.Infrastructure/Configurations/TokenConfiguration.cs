namespace Aralash.Infrastructure.Configurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(t => new { t.RefreshToken, t.UserId });
        builder.Property(t => t.RefreshToken).IsRequired();
        builder.Property(t => t.UserId).IsRequired();

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
    }
}