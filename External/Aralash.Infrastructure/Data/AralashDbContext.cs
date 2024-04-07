using Aralash.Domain.Abstractions;
using Aralash.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Aralash.Infrastructure.Data;

public class AralashDbContext : DbContext, IAralashDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<SecuredOperation> SecuredOperations { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RoleOperation> RoleOperations { get; set; }
    
    public AralashDbContext(DbContextOptions<AralashDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AralashDbContext).Assembly);
    }
}