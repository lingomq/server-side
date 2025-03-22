using AutoMapper;
using LingoMQ.Core.Application.Features.Words;
using LingoMQ.Core.Domain.Words;
using LingoMQ.Core.Domain.Words.Events;
using MediatR;

namespace LingoMQ.Core.Application.Words.Commands;

public class AddWordTranslationCommand : IRequest<WordInfoDto>
{
    public Guid WordId { get; set; }
    public WordInfoDto Translation { get; set; }

    public AddWordTranslationCommand(Guid id, WordInfoDto translationDto)
    {
        WordId = id;
        Translation = translationDto;
    }
}

public class AddWordTranslationCommandHandler
    : IRequestHandler<AddWordTranslationCommand, WordInfoDto>
{
    private readonly IWordsUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWordInfoRepository _wordInfoRepository;

    public AddWordTranslationCommandHandler(
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
        AddWordTranslationCommand request,
        CancellationToken cancellationToken
    )
    {
        var word =
            await _wordInfoRepository.FindAsync(x => x.Id == request.WordId, cancellationToken)
            ?? throw new InvalidDataException("The word wasn't found");

        word.AddTranslation(_mapper.Map<WordInfo>(request.Translation));
        word.AddEvent(new AddedWordTranslationEvent(word));

        await _wordInfoRepository.UpdateAsync(word, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<WordInfoDto>(word);
    }
}
