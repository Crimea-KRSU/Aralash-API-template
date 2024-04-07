using Aralash.App.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Aralash.Presentation.Controllers;

[Authorize]
[Route("api/roles")]
[EnableCors("CorsPolicy")]
public class RoleController : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult> CreateRole([FromServices] IMediator mediator, 
        [FromBody] CreateRoleCommand request, CancellationToken ct)
    {
        var id = await mediator.Send(request, ct);
        return Ok(id);
    }
    
    [HttpPost("add-sec-op")]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult> AddSecuredOperationToRole([FromServices] IMediator mediator, 
        [FromBody] AddSecuredOperationToRoleCommand request, CancellationToken ct)
    {
        await mediator.Send(request, ct);
        return Ok();
    }
    
    [HttpGet("list")]
    [ProducesResponseType<ListQueryResult<RoleView>>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<ListQueryResult<RoleView>>> GetRoles([FromServices] IMediator mediator, 
        [FromQuery] GetRolesQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }
    
    [HttpPost("add-to-user")]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult> AttachRoleToUser([FromServices] IMediator mediator, 
        [FromBody] AttachRoleToUserCommand request, CancellationToken ct)
    {
        await mediator.Send(request, ct);
        return Ok();
    }
    
    [HttpDelete("delete")]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult> DeleteRole([FromServices] IMediator mediator, 
        [FromBody] DeleteRoleCommand request, CancellationToken ct)
    {
        await mediator.Send(request, ct);
        return Ok();
    }
    
    [HttpDelete("delete-from-user")]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult> DetachRoleFromUser([FromServices] IMediator mediator, 
        [FromBody] DetachRoleFromUserCommand request, CancellationToken ct)
    {
        await mediator.Send(request, ct);
        return Ok();
    }
    
    [HttpDelete("delete-sec-op")]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult> DeleteSecuredOperationFromRole([FromServices] IMediator mediator, 
        [FromBody] DeleteSecuredOperationFromRoleCommand request, CancellationToken ct)
    {
        await mediator.Send(request, ct);
        return Ok();
    }
    
    [HttpGet("list-sec-op")]
    [ProducesResponseType<ListQueryResult<SecuredOperationView>>(200)]
    [ProducesResponseType<AuthFailure>(403)]
    [ProducesResponseType<ExceptionFailure>(500)]
    public async Task<ActionResult<ListQueryResult<SecuredOperationView>>> GetRoles([FromServices] IMediator mediator, 
        [FromQuery] GetSecuredOperationsQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }
}