using System.Security.Claims;
using LingoMQ.Core.Application.Features.Users;
using LingoMQ.Core.Application.Features.Users.ChangeUserDescription;
using LingoMQ.Core.Application.Features.Users.ChangeUserEmail;
using LingoMQ.Core.Application.Features.Users.ChangeUserNickname;
using LingoMQ.Core.Application.Features.Users.ChangeUserPassword;
using LingoMQ.Core.Application.Features.Users.ChangeUserRole;
using LingoMQ.Core.Application.Features.Users.GetUser;
using LingoMQ.Core.Application.Features.Users.GetUserEmail;
using LingoMQ.Core.Application.Features.Users.GetUserImage;
using LingoMQ.Core.Application.Features.Users.UploadImage;
using LingoMQ.Presenters.WebApi.Constants;
using LingoMQ.Presenters.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LingoMQ.Presenters.WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    private Guid UserId =>
        new Guid(
            User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value
                ?? Guid.NewGuid().ToString()
        );

    [HttpGet]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery() { UserId = UserId }, cancellationToken);
        return Ok(user);
    }

    [HttpGet("image")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> GetUserImage(Guid userId, CancellationToken cancellationToken)
    {
        if (userId.Equals(Guid.Empty))
            userId = UserId;

        var imagePath = await mediator.Send(
            new GetUserImageQuery() { UserId = userId },
            cancellationToken
        );
        return File(System.IO.File.ReadAllBytes(imagePath), "image/jpeg");
    }

    [HttpGet("email")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> GetUserEmail(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetUserEmailQuery() { Id = UserId },
            cancellationToken
        );
        return Ok(result);
    }

    [HttpPatch("image")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> UploadImage(
        [FromForm] UploadImageRequest request,
        CancellationToken cancellationToken
    )
    {
        var imageId = await mediator.Send(
            new UploadImageCommand(UserId, request.X, request.Y),
            cancellationToken
        );
        string path =
            Directory.GetCurrentDirectory() + "/Binaries/Images/Users/" + imageId + ".png";
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await request.File.CopyToAsync(fileStream);
        }

        return Accepted();
    }

    [HttpPatch("nickname/{nickname}")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> ChangeNickname(
        string nickname,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(
            new ChangeUserNicknameCommand() { Id = UserId, Nickname = nickname },
            cancellationToken
        );
        return Accepted();
    }

    [HttpPatch("description")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> ChangeDescription(
        ChangeUserDescriptionModel request,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(
            new ChangeUserDescriptionCommand() { Id = UserId, Description = request.Description },
            cancellationToken
        );
        return Accepted();
    }

    [HttpPatch("email")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> ChangeEmail(
        ChangeUserEmailModel request,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(
            new ChangeUserEmailCommand() { Id = UserId, Email = request.Email },
            cancellationToken
        );
        return Accepted();
    }

    [HttpPatch("password")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordModel model,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(
            new ChangeUserPasswordCommand()
            {
                UserId = UserId,
                Password = model.Password,
                OldPassword = model.OldPassword,
            },
            cancellationToken
        );
        return Accepted();
    }

    [HttpPatch("role/{userId}")]
    [Authorize(Roles = AuthorizationRoles.Everyone)]
    public async Task<IActionResult> ChangeRole(
        Guid userId,
        UserRoleDto userRole,
        CancellationToken cancellationToken
    )
    {
        await mediator.Send(
            new ChangeUserRoleCommand()
            {
                UserId = userId,
                ChangerId = UserId,
                Role = userRole,
            },
            cancellationToken
        );
        return Accepted();
    }
}
