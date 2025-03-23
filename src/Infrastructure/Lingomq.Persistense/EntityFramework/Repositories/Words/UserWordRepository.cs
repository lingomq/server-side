using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using LingoMQ.Core.Domain.Words;
using LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words.Models;
using Microsoft.EntityFrameworkCore;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words;

public class UserWordRepository : IUserWordRepository
{
    private readonly WordsDbContext _wordsDbContext;
    private readonly IMapper _mapper;

    public UserWordRepository(WordsDbContext wordsDbContext, IMapper mapper)
    {
        _wordsDbContext = wordsDbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(UserWord entity, CancellationToken cancellationToken = default)
    {
        var dao = new UserWordDao() { UserId = entity.User.Id, WordId = entity.Word.Id };
        await _wordsDbContext.UserWords.AddAsync(dao, cancellationToken);
    }

    public async Task<UserWord?> FindAsync(
        Expression<Func<UserWord, bool>>? spec = null,
        CancellationToken cancellationToken = default
    )
    {
        var expression = _mapper.MapExpression<Expression<Func<UserWordDao, bool>>>(spec);
        var result = spec is not null
            ? await _wordsDbContext
                .UserWords.OrderBy(x => x.Word)
                .Where(expression)
                .FirstOrDefaultAsync(expression, cancellationToken)
            : await _wordsDbContext.UserWords.Order().FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<UserWord?>(result);
    }

    public async Task<IEnumerable<UserWord>> GetAsync(
        Expression<Func<UserWord, bool>>? spec = null,
        int take = int.MaxValue,
        int skip = 0,
        CancellationToken cancellationToken = default
    )
    {
        var expression = _mapper.MapExpression<Expression<Func<UserWordDao, bool>>>(spec);
        var result = spec is not null
            ? await _wordsDbContext
                .UserWords.OrderBy(x => x.Word)
                .Take(take)
                .Skip(skip)
                .Where(expression)
                .ToListAsync(cancellationToken)
            : await _wordsDbContext
                .UserWords.OrderBy(x => x.Word)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        return _mapper.Map<IEnumerable<UserWord>>(result);
    }

    public async Task RemoveAsync(UserWord entity, CancellationToken cancellationToken = default)
    {
        var dao = await _wordsDbContext.UserWords.FirstAsync(x => x.Id == entity.Id, cancellationToken);
        await Task.Run(() => _wordsDbContext.UserWords.Remove(dao));
    }

    public async Task UpdateAsync(UserWord entity, CancellationToken cancellationToken = default)
    {
        var dao = await _wordsDbContext.UserWords.FirstAsync(x => x.Id == entity.Id, cancellationToken);
        await Task.Run(() => _wordsDbContext.UserWords.Update(dao));
    }
}
