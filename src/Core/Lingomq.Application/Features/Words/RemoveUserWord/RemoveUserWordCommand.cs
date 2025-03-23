using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.RemoveUserWord;

public class RemoveUserWordCommand : IRequest<UserWordDto>
{
    public Guid UserWordId { get; set; }

    public RemoveUserWordCommand(Guid userWordId)
    {
        UserWordId = userWordId;
    }
}

public class RemoveUserWordCommandHandler : IRequestHandler<RemoveUserWordCommand, UserWordDto>
{
    private IUserWordRepository _userWordRepository;
    private IWordsUnitOfWork _wordsUnitOfWork;
    private IMapper _mapper;

    public RemoveUserWordCommandHandler(
        IUserWordRepository userWordRepository,
        IWordsUnitOfWork wordsUnitOfWork,
        IMapper mapper
    )
    {
        _userWordRepository = userWordRepository;
        _wordsUnitOfWork = wordsUnitOfWork;
        _mapper = mapper;
    }

    public async Task<UserWordDto> Handle(
        RemoveUserWordCommand request,
        CancellationToken cancellationToken
    )
    {
        var userWord =
            await _userWordRepository.FindAsync(x => x.Id == request.UserWordId, cancellationToken)
            ?? throw new InvalidDataException("UserWord wasn't found");
        await _userWordRepository.RemoveAsync(userWord, cancellationToken);
        await _wordsUnitOfWork.CommitAsync(cancellationToken);

        var result = _mapper.Map<UserWordDto>(userWord);

        return result;
    }
}
