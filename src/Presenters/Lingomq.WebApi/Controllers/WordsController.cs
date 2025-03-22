using LingoMQ.Core.Application.Words;
using LingoMQ.Core.Application.Words.Commands;
using LingoMQ.Core.Application.Words.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LingoMQ.Presenters.WebApi.Controllers;

[Route("api/words")]
[ApiController]
public class WordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{language}/{code}/{subCode}")]
    public async Task<IActionResult> Get(
        string language,
        string code,
        string subCode,
        CancellationToken cancellationToken,
        string thematics = "general",
        string searchedWord = "",
        int take = int.MaxValue,
        int skip = 0
    )
    {
        var getWordsCommand = new GetWordsQuery()
        {
            Take = take,
            Skip = skip,
            Thematics = new() { Category = thematics },
            Language = new()
            {
                Value = language,
                Code = code,
                SubCode = subCode,
            },
            SearchedWord = searchedWord,
        };

        var result = await _mediator.Send(getWordsCommand, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        AddWordsCommand addWordsCommand,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(addWordsCommand, cancellationToken);
        return Accepted();
    }

    [HttpPost("from-file")]
    public async Task<IActionResult> AddFromFile(
        IFormFile file,
        CancellationToken cancellationToken
    )
    {
        string uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Path.Exists(uploads))
            Directory.CreateDirectory(uploads);
        string filePath = Path.Combine(uploads, file.FileName);
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream, cancellationToken);
        }

        await _mediator.Send(new AddWordsFromFileCommand(filePath), cancellationToken);

        return Accepted();
    }

    [HttpPatch("translations/add/{wordId}")]
    public async Task<IActionResult> AddWordTranslation(
        Guid wordId,
        WordInfoDto wordTranslation,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new AddWordTranslationCommand(wordId, wordTranslation),
            cancellationToken
        );
        return Accepted(result);
    }

    [HttpPatch("change-thematics/{wordId}")]
    public async Task<IActionResult> ChangeWordThematics(
        Guid wordId,
        WordThematicsDto wordThematics,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new ChangeWordThematicsCommand(wordId, wordThematics),
            cancellationToken
        );
        return Accepted(result);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveWords(
        [FromQuery] Guid[] ids,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new RemoveWordsCommand(ids), cancellationToken);
        return Accepted();
    }
}
