using AutoMapper;
using LingoMQ.Core.Application.Services.Parser;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.AddWordsFromFile;

public class AddWordsFromFileCommand : IRequest
{
    public string FilePath { get; set; }

    public AddWordsFromFileCommand(string filePath)
    {
        FilePath = filePath;
    }
}

public class AddWordsFromFileCommandHandler : IRequestHandler<AddWordsFromFileCommand>
{
    private readonly IWordsUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWordInfoRepository _wordInfoRepository;

    public AddWordsFromFileCommandHandler(
        IWordInfoRepository wordInfoRepository,
        IWordsUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _wordInfoRepository = wordInfoRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(AddWordsFromFileCommand request, CancellationToken cancellationToken)
    {
        var fileParser = FileParcerBuilder.BuildInstanse(request.FilePath.Split('.')[^1]);
        var parsedEntities = fileParser.ParseData<WordInfoDto>(request.FilePath, cancellationToken);

        var mappedEntities = _mapper.Map<List<WordInfo>>(parsedEntities);
        foreach (var entity in mappedEntities)
            await _wordInfoRepository.AddAsync(entity, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
