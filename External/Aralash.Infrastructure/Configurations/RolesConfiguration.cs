namespace Aralash.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData([
            new Role()
            {
                Id = "absolute-1337",
                Description = "Супер админ системы",
                Name = "Супер админ"
            }
        ]);
    }
}