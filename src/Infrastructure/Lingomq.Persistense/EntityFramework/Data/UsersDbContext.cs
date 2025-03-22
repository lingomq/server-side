using System.Reflection;
using LingoMQ.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework;

public class UsersDbContext : DbContext
{
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<UserCredentials> UserCredentials => Set<UserCredentials>();
    public virtual DbSet<UserImage> UserImages => Set<UserImage>();

    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
