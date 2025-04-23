using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.RemoveUserWordByWord;

public class RemoveUserWordByWordCommand : IRequest
{
    public Guid WordId { get; set; }
    public Guid UserId { get; set; }

    public RemoveUserWordByWordCommand(Guid wordId, Guid userId)
    {
        WordId = wordId;
        UserId = userId;
    }
}

public class RemoveUserWordByWordCommandHandler : IRequestHandler<RemoveUserWordByWordCommand>
{
    private readonly IUserWordRepository _userWordRepository;
    private readonly IWordsUnitOfWork _wordsUnitOfWork;

    public RemoveUserWordByWordCommandHandler(
        IUserWordRepository userWordRepository,
        IWordsUnitOfWork wordsUnitOfWork
    )
    {
        _userWordRepository = userWordRepository;
        _wordsUnitOfWork = wordsUnitOfWork;
    }

    public async Task Handle(
        RemoveUserWordByWordCommand request,
        CancellationToken cancellationToken
    )
    {
        var userWord =
            await _userWordRepository.FindAsync(
                x => x.User.Id == request.UserId && x.Word.Id == request.WordId,
                cancellationToken
            ) ?? throw new InvalidDataException("User word wasn't found");

        await _userWordRepository.RemoveAsync(userWord, cancellationToken);
        await _wordsUnitOfWork.CommitAsync(cancellationToken);
    }
}
