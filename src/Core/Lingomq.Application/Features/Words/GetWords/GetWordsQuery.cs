using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.GetWords;

public class GetWordsQuery : IRequest<IEnumerable<WordInfoDto>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = int.MaxValue;
    public WordThematicsDto? Thematics { get; set; }
    public required LanguageDto Language { get; set; }
    public LanguageDto? LanguageTo { get; set; }
    public string SearchedWord { get; set; } = "";
}

public class GetWordsQueryHandler : IRequestHandler<GetWordsQuery, IEnumerable<WordInfoDto>>
{
    private readonly IMapper _mapper;
    private readonly IWordInfoRepository _wordInfoRepository;

    public GetWordsQueryHandler(IWordInfoRepository wordInfoRepository, IMapper mapper)
    {
        _wordInfoRepository = wordInfoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WordInfoDto>> Handle(
        GetWordsQuery request,
        CancellationToken cancellationToken
    )
    {
        var language = _mapper.Map<Language>(request.Language);
        var languageTo = _mapper.Map<Language>(request.LanguageTo);
        var thematics = _mapper.Map<WordThematics>(request.Thematics);
        var entities = await _wordInfoRepository.GetAsync(
            new IsSpecifiedWordByFilterSpecification(
                language,
                thematics,
                request.SearchedWord,
                languageTo
            ).ToExpression(),
            cancellationToken: cancellationToken,
            take: request.Take,
            skip: request.Skip
        );

        return _mapper.Map<IEnumerable<WordInfoDto>>(entities);
    }
}
