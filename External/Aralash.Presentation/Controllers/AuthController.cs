using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Aralash.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
[EnableCors("CorsPolicy")]
public class AuthController : ControllerBase
{

    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType<TokenDto>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<TokenDto>> Login([FromServices] IMediator mediator, 
        [FromBody] LoginCommand loginCommand,
        CancellationToken ct)
    {
        var result = await mediator.Send(loginCommand, ct);
        return Ok(result);
    }
    
    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType<TokenDto>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<TokenDto>> Register([FromServices] IMediator mediator, 
        [FromBody] RegisterCommand registerCommand,
        CancellationToken ct)
    {
        var result = await mediator.Send(registerCommand, ct);
        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("logout")]
    [Produces("application/json")]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<TokenDto>> Logout([FromServices] IMediator mediator, 
        [FromBody] string refreshToken,
        CancellationToken ct)
    {
        await mediator.Send(new LogoutCommand(refreshToken), ct);
        return Ok();
    }

    [HttpPost("refresh")]
    [Produces("application/json")]
    [ProducesResponseType<TokenDto>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<TokenDto>> Refresh([FromServices] IMediator mediator,
        [FromBody] RefreshCommand refreshCommand,
        CancellationToken ct)
    {
        var result = await mediator.Send(refreshCommand, ct);
        return Ok(result);
    }
}