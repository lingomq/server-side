using System.Reflection;
using LingoMQ.Core.Domain.Words;
using Microsoft.EntityFrameworkCore;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework;

public class WordsDbContext : DbContext 
{
    public virtual DbSet<WordInfo> WordInfos => Set<WordInfo>();
    public WordsDbContext(DbContextOptions<WordsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    
}
