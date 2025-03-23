using System.Linq.Expressions;
using LingoMQ.Core.Domain.Words;
using Microsoft.EntityFrameworkCore;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words;

public class WordInfoRepository : IWordInfoRepository
{
    private readonly WordsDbContext _context;

    public WordInfoRepository(WordsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WordInfo>> GetAsync(
        Expression<Func<WordInfo, bool>>? spec = null,
        int take = int.MaxValue,
        int skip = 0,
        CancellationToken cancellationToken = default
    )
    {
        return spec is not null
            ? await _context
                .Set<WordInfo>()
                .OrderBy(x => x.Word)
                .Where(spec)
                .Take(take)
                .Skip(skip)
                .ToListAsync(cancellationToken)
            : await _context
                .Set<WordInfo>()
                .OrderBy(x => x.Word)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
    }

    public async Task AddAsync(WordInfo entity, CancellationToken cancellationToken = default) =>
        await _context.Set<WordInfo>().AddAsync(entity, cancellationToken);

    public async Task<WordInfo?> FindAsync(
        Expression<Func<WordInfo, bool>>? spec = null,
        CancellationToken cancellationToken = default
    )
    {
        return spec is not null
            ? await _context.Set<WordInfo>().Order().FirstOrDefaultAsync(spec, cancellationToken)
            : await _context.Set<WordInfo>().Order().FirstOrDefaultAsync(cancellationToken);
    }

    public Task RemoveAsync(WordInfo entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _context.Set<WordInfo>().Remove(entity));
    }

    public Task UpdateAsync(WordInfo entity, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _context.Set<WordInfo>().Update(entity));
    }
}
