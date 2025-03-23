using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.AddWord;

public class AddWordsCommand : IRequest
{
    public IEnumerable<WordInfoDto> Words { get; set; }

    public AddWordsCommand(IEnumerable<WordInfoDto> words) => Words = words;
}

public class AddWordsCommandHandler : IRequestHandler<AddWordsCommand>
{
    private readonly IWordsUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWordInfoRepository _wordInfoRepository;

    public AddWordsCommandHandler(
        IWordInfoRepository wordInfoRepository,
        IWordsUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _wordInfoRepository = wordInfoRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(AddWordsCommand request, CancellationToken cancellationToken)
    {
        var mappedEntities = _mapper.Map<IEnumerable<WordInfo>>(request.Words);
        foreach (var entity in mappedEntities)
            await _wordInfoRepository.AddAsync(entity, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
