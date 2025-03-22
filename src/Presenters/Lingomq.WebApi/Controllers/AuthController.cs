using LingoMQ.Core.Application.Features.Auth.SignIn;
using LingoMQ.Core.Application.Features.Users.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LingoMQ.Presenters.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(
        SignInModel signModel,
        CancellationToken cancellationToken
    )
    {
        var tokens = await mediator.Send(
            new SignInCommand() { SignModel = signModel },
            cancellationToken
        );
        return Ok(tokens);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(
        CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(command, cancellationToken);
        return Accepted();
    }
}
