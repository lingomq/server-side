using AutoMapper;
using LingoMQ.Core.Application.Features.Words;
using LingoMQ.Core.Domain.Words;
using LingoMQ.Core.Domain.Words.Events;
using MediatR;

namespace LingoMQ.Core.Application.Words.Commands;

public class ChangeWordThematicsCommand : IRequest<WordInfoDto>
{
    public Guid WordId { get; set; }
    public WordThematicsDto WordThematics { get; set; }

    public ChangeWordThematicsCommand(Guid wordId, WordThematicsDto wordThematics)
    {
        WordId = wordId;
        WordThematics = wordThematics;
    }
}

public class ChangeWordThematicsCommandHandler
    : IRequestHandler<ChangeWordThematicsCommand, WordInfoDto>
{
    private readonly IWordsUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWordInfoRepository _wordInfoRepository;

    public ChangeWordThematicsCommandHandler(
        IWordsUnitOfWork unitOfWork,
        IMapper mapper,
        IWordInfoRepository wordInfoRepository
    )
    {
        _wordInfoRepository = wordInfoRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WordInfoDto> Handle(
        ChangeWordThematicsCommand request,
        CancellationToken cancellationToken
    )
    {
        var word =
            await _wordInfoRepository.FindAsync(x => x.Id == request.WordId, cancellationToken)
            ?? throw new InvalidDataException("The word wasn't found");

        word.ChangeThematics(_mapper.Map<WordThematics>(request.WordThematics));
        word.AddEvent(new WordThematicsWasChangedEvent(word));

        await _wordInfoRepository.UpdateAsync(word, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<WordInfoDto>(word);
    }
}
