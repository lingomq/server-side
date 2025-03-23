using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.GetUserWords;

public class GetUserWordsQuery : IRequest<IEnumerable<UserWordDto>>
{
    public Guid UserId { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }

    public GetUserWordsQuery(Guid userId, int take, int skip)
    {
        UserId = userId;
        Take = take;
        Skip = skip;
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
            x => x.User.Id == request.UserId,
            request.Take,
            request.Skip,
            cancellationToken
        );

        var result = _mapper.Map<IEnumerable<UserWordDto>>(userWords);

        return result;
    }
}
