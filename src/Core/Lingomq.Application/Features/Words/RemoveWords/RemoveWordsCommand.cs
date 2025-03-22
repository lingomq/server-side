using LingoMQ.Core.Application.Features.Words;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Words.Commands;

public class RemoveWordsCommand : IRequest
{
    public IEnumerable<Guid> Ids { get; set; }

    public RemoveWordsCommand(IEnumerable<Guid> ids)
    {
        Ids = ids;
    }
}

public class RemoveWordsCommandHandler : IRequestHandler<RemoveWordsCommand>
{
    private readonly IWordsUnitOfWork _unitOfWork;
    private readonly IWordInfoRepository _wordInfoRepository;

    public RemoveWordsCommandHandler(IWordsUnitOfWork unitOfWork, IWordInfoRepository wordInfoRepository)
    {
        _unitOfWork = unitOfWork;
        _wordInfoRepository = wordInfoRepository;
    }

    public async Task Handle(RemoveWordsCommand request, CancellationToken cancellationToken)
    {
        var entitiesToRemove = await _wordInfoRepository.GetAsync(
            x => request.Ids.Contains(x.Id),
            cancellationToken: cancellationToken
        );

        foreach (var entity in entitiesToRemove)
            await _wordInfoRepository.RemoveAsync(entity, cancellationToken);
            
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
