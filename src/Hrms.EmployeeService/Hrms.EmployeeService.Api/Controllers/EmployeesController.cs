using Hrms.EmployeeService.Api.Models;
using Hrms.EmployeeService.Application.UseCases.CreateEmployee;
using Hrms.EmployeeService.Application.UseCases.GetAllEmployees;
using Hrms.EmployeeService.Application.UseCases.GetEmployee;
using Hrms.EmployeeService.Application.UseCases.TerminateEmployee;
using Hrms.EmployeeService.Application.UseCases.UpdateEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.EmployeeService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class EmployeesController(
    IMediator mediator,
    ILogger<EmployeesController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "EmployeeRead")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "EmployeeRead")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEmployeeQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost]
    [Authorize(Policy = "EmployeeWrite")]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateEmployeeCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Department,
            request.Position,
            request.HireDate,
            request.Salary
        );

        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        logger.LogInformation("Employee created: {Id}", result.Data?.Id);

        return CreatedAtAction(
            nameof(Get),
            new { id = result.Data!.Id },
            result.Data);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "EmployeeWrite")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(
            id,
            request.FirstName,
            request.LastName,
            request.PhoneNumber
        );

        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "EmployeeWrite")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new TerminateEmployeeCommand(id, "Administrative termination");
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpGet("debug")]
    [AllowAnonymous] // optional: remove if you want it protected
    public IActionResult DebugClaims()
    {
        var claims = User.Claims
            .Select(c => new { c.Type, c.Value })
            .ToList();

        return Ok(claims);
    }

}