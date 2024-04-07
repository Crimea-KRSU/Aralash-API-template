namespace Aralash.Domain.Abstractions;

public interface IAralashDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<SecuredOperation> SecuredOperations { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RoleOperation> RoleOperations { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    int SaveChanges();
}