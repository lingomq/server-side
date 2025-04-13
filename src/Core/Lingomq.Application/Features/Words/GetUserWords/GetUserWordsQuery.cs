using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.GetUserWords;

public class GetUserWordsQuery : IRequest<IEnumerable<UserWordDto>>
{
    public Guid UserId { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
    public LanguageDto Language { get; set; }
    public string Thematics { get; set; } = "general";
    public string SearchedWord { get; set; } = "";

    public GetUserWordsQuery(Guid userId, int take, int skip)
    {
        UserId = userId;
        Take = take;
        Skip = skip;
        Language = new()
        {
            Value = "english",
            Code = "en",
            SubCode = "US",
        };
    }

    public GetUserWordsQuery(
        Guid userId,
        int take,
        int skip,
        LanguageDto language,
        string thematics,
        string searchedWord
    )
    {
        UserId = userId;
        Take = take;
        Skip = skip;
        Language = language;
        Thematics = thematics;
        SearchedWord = searchedWord;
    }
}

public class GetUserWordsQueryHandler : IRequestHandler<GetUserWordsQuery, IEnumerable<UserWordDto>>
{
    private readonly IUserWordRepository _userWordRepository;
    private readonly IMapper _mapper;

    public GetUserWordsQueryHandler(IUserWordRepository userWordRepository, IMapper mapper)
    {
        _userWordRepository = userWordRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserWordDto>> Handle(
        GetUserWordsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userWords = await _userWordRepository.GetAsync(
            x =>
                x.User.Id == request.UserId
                && (
                    x.Word.Language.Value == request.Language.Value
                    && x.Word.Language.Code == request.Language.Code
                    && x.Word.Language.SubCode == request.Language.SubCode
                    && x.Word.Word.Contains(request.SearchedWord)
                )
                && (
                    x.Word.Thematics.Category.Contains(
                        request.Thematics == "general" ? "" : request.Thematics
                    )
                ),
            request.Take,
            request.Skip,
            cancellationToken
        );

        var result = _mapper.Map<IEnumerable<UserWordDto>>(userWords);

        return result;
    }
}
