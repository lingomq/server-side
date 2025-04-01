using AutoMapper;
using LingoMQ.Core.Domain.Words;
using MediatR;

namespace LingoMQ.Core.Application.Features.Words.CheckTranslation;

public class CheckTranslationCommand : IRequest<bool>
{
    public CheckTranslationRequest Answer { get; set; }

    public CheckTranslationCommand(CheckTranslationRequest answer)
    {
        Answer = answer;
    }
}

public class CheckTranslationCommandHandler : IRequestHandler<CheckTranslationCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IWordInfoRepository _wordInfoRepository;

    public CheckTranslationCommandHandler(IMapper mapper, IWordInfoRepository wordInfoRepository)
    {
        _mapper = mapper;
        _wordInfoRepository = wordInfoRepository;
    }

    public async Task<bool> Handle(
        CheckTranslationCommand request,
        CancellationToken cancellationToken
    )
    {
        var word =
            await _wordInfoRepository.FindAsync(
                x => x.Id == request.Answer.WordId,
                cancellationToken
            ) ?? throw new InvalidDataException("Word Info wasn't found");

        return word.Translations.Any(x =>
            x.Word.Equals(request.Answer.Translation)
            && x.Language.Code == request.Answer.LanguageTo.Code
            && x.Language.SubCode == request.Answer.LanguageTo.SubCode
            && x.Language.Value == request.Answer.LanguageTo.Value
        );
    }
}
