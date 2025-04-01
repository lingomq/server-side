using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.GetRandomUserWords;

public class GetRandomUserWordsQuery : IRequest<IEnumerable<UserWordDto>>
{
    public int Limit { get; set; }
    public Guid UserId { get; set; }

    public GetRandomUserWordsQuery(Guid userId, int limit)
    {
        Limit = limit;
        UserId = userId;
    }
}

public class GetRandomUserWordsQueryHandler
    : IRequestHandler<GetRandomUserWordsQuery, IEnumerable<UserWordDto>>
{
    private readonly IMapper _mapper;
    private readonly IUserWordRepository _userWordRepository;

    public GetRandomUserWordsQueryHandler(IMapper mapper, IUserWordRepository userWordRepository)
    {
        _mapper = mapper;
        _userWordRepository = userWordRepository;
    }

    public async Task<IEnumerable<UserWordDto>> Handle(
        GetRandomUserWordsQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _userWordRepository.GetRandomUserWordsAsync(
            request.UserId,
            limit: request.Limit,
            cancellationToken: cancellationToken
        );

        return _mapper.Map<IEnumerable<UserWordDto>>(result).DistinctBy(x => x.Word.Id);
    }
}
