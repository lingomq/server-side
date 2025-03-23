using AutoMapper;
using LingoMQ.Core.Domain.Users;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.AddUserWord;

public class AddUserWordCommand : IRequest<UserWordDto>
{
    public AddUserWordRequest Request { get; set; }

    public AddUserWordCommand(AddUserWordRequest request)
    {
        Request = request;
    }
}

public class AddUserWordCommandHandler : IRequestHandler<AddUserWordCommand, UserWordDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IWordInfoRepository _wordInfoRepository;
    private readonly IUserWordRepository _userWordRepository;
    private readonly IWordsUnitOfWork _wordsUnitOfWork;
    private readonly IMapper _mapper;

    public AddUserWordCommandHandler(
        IUserRepository userRepository,
        IWordInfoRepository wordInfoRepository,
        IUserWordRepository userWordRepository,
        IWordsUnitOfWork wordsUnitOfWork,
        IMapper mapper
    )
    {
        _userRepository = userRepository;
        _wordInfoRepository = wordInfoRepository;
        _userWordRepository = userWordRepository;
        _wordsUnitOfWork = wordsUnitOfWork;
        _mapper = mapper;
    }

    public async Task<UserWordDto> Handle(
        AddUserWordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindAsync(x => x.Id == request.Request.UserId, cancellationToken)
            ?? throw new InvalidDataException("User wasn't found");
        var word =
            await _wordInfoRepository.FindAsync(
                x => x.Id == request.Request.WordId,
                cancellationToken
            ) ?? throw new InvalidDataException("Word wasn't found");

        var userWord = new UserWord(user, word);
        await _userWordRepository.AddAsync(userWord, cancellationToken);
        await _wordsUnitOfWork.CommitAsync(cancellationToken);

        var result = _mapper.Map<UserWordDto>(userWord);

        return result;
    }
}
