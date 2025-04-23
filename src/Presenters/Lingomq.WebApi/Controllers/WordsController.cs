using System.Security.Claims;
using LingoMQ.Core.Application.Features.Words;
using LingoMQ.Core.Application.Features.Words.AddUserWord;
using LingoMQ.Core.Application.Features.Words.AddWord;
using LingoMQ.Core.Application.Features.Words.AddWordsFromFile;
using LingoMQ.Core.Application.Features.Words.AddWordTranslation;
using LingoMQ.Core.Application.Features.Words.ChangeWordThematics;
using LingoMQ.Core.Application.Features.Words.CheckTranslation;
using LingoMQ.Core.Application.Features.Words.GetRandomUserWords;
using LingoMQ.Core.Application.Features.Words.GetUserWords;
using LingoMQ.Core.Application.Features.Words.GetWords;
using LingoMQ.Core.Application.Features.Words.RemoveUserWord;
using LingoMQ.Core.Application.Features.Words.RemoveUserWordByWord;
using LingoMQ.Core.Application.Features.Words.RemoveWords;
using LingoMQ.Presenters.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LingoMQ.Presenters.WebApi.Controllers;

[Route("api/words")]
[ApiController]
public class WordsController : ControllerBase
{
    private Guid UserId =>
        new Guid(
            User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value
                ?? Guid.NewGuid().ToString()
        );

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
        string? languageTo,
        string? codeTo,
        string? subCodeTo,
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
            LanguageTo = languageTo is null
                ? null
                : new()
                {
                    Value = languageTo,
                    Code = codeTo!,
                    SubCode = subCodeTo!,
                },
            SearchedWord = searchedWord,
        };

        var result = await _mediator.Send(getWordsCommand, cancellationToken);

        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserWords(
        Guid? userId,
        CancellationToken cancellationToken = default,
        int take = 20,
        int skip = 0,
        string language = "english",
        string code = "en",
        string subCode = "US",
        string searchedWord = "",
        string thematics = "general"
    )
    {
        if (userId is null)
            userId = UserId;

        var result = await _mediator.Send(
            new GetUserWordsQuery(
                (Guid)userId,
                take,
                skip,
                new()
                {
                    Value = language,
                    Code = code,
                    SubCode = subCode,
                },
                thematics,
                searchedWord
            ),
            cancellationToken
        );
        return Ok(result);
    }

    [HttpGet("random/{limit}")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> GetRandomUserWords(
        int limit = 5,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(
            new GetRandomUserWordsQuery(UserId, limit),
            cancellationToken
        );
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

    [HttpPost("user")]
    public async Task<IActionResult> AddUserWord(
        AddUserWordRequest request,
        CancellationToken cancellationToken
    )
    {
        request.UserId = UserId;
        var result = await _mediator.Send(new AddUserWordCommand(request), cancellationToken);
        return Accepted(result);
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

    [HttpPost("translation-validator")]
    public async Task<IActionResult> ValidateTranslation(
        CheckTranslationRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new CheckTranslationCommand(request), cancellationToken);
        return result ? Ok() : BadRequest();
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

    [HttpDelete("user-word")]
    public async Task<IActionResult> RemoveUserWord(
        [FromQuery] Guid userWordId,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new RemoveUserWordCommand(userWordId), cancellationToken);
        return Accepted(result);
    }

    [HttpDelete("user/{wordId}")]
    public async Task<IActionResult> RemoveUserWordByWord(
        Guid wordId,
        CancellationToken cancellationToken = default
    )
    {
        await _mediator.Send(new RemoveUserWordByWordCommand(wordId, UserId), cancellationToken);
        return Accepted();
    }
}
