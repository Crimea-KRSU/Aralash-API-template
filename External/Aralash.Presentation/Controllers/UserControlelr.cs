using Aralash.App.User.GetUserInfo;
using Aralash.Domain.Abstractions.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Aralash.Presentation.Controllers;

[Authorize]
[Route("api/user")]
[EnableCors("CorsPolicy")]
public class UserController : ControllerBase
{
    [HttpGet("info")]
    [ProducesResponseType<UserView>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<UserView>> AboutOther([FromServices] IMediator mediator,
        [FromQuery] GetUserInfoCommand request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }
    
    [HttpGet("me")]
    [ProducesResponseType<UserView>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<UserView>> AboutMe([FromServices] IMediator mediator,
        [FromServices] ICurrentUser currentUser, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserInfoCommand(currentUser.UserId!), ct);
        return Ok(result);
    }
    
    [HttpGet("list")]
    [ProducesResponseType<ListQueryResult<UserView>>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<ListQueryResult<UserView>>> ListUsers([FromServices] IMediator mediator,
        [FromQuery] GetUsersCommand request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }
}