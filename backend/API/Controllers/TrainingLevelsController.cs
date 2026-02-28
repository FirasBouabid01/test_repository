using API.Common;
using API.Middleware;
using Application.TrainingLevels.Commands.CreateTrainingLevel;
using Application.TrainingLevels.Commands.DeleteTrainingLevel;
using Application.TrainingLevels.Commands.ReorderLevels;
using Application.TrainingLevels.Commands.UpdateTrainingLevel;
using Application.TrainingLevels.Queries.GetAllTrainingLevels;
using Application.TrainingLevels.Queries.GetLevelHierarchy;
using Application.TrainingLevels.Queries.GetTrainingLevelById;
using Application.TrainingLevels.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/training-levels")]
[Authorize]
public class TrainingLevelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrainingLevelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission("TrainingLevel.Read")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] bool? isActive = null)
    {
        var query = new GetAllTrainingLevelsQuery(pageNumber, pageSize, isActive);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.CreateSuccess(result));
    }

    [HttpGet("{id}")]
    [HasPermission("TrainingLevel.Read")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTrainingLevelByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound(ApiResponse<object>.CreateError("Training level not found", 404));

        return Ok(ApiResponse<TrainingLevelDto>.CreateSuccess(result));
    }

    [HttpGet("hierarchy")]
    [HasPermission("TrainingLevel.Read")]
    public async Task<IActionResult> GetHierarchy()
    {
        var query = new GetLevelHierarchyQuery();
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<IEnumerable<TrainingLevelHierarchyDto>>.CreateSuccess(result));
    }

    [HttpPost]
    [HasPermission("TrainingLevel.Create")]
    public async Task<IActionResult> Create([FromBody] CreateTrainingLevelCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<TrainingLevelDto>.CreateSuccess(result, 201));
        }
        catch (Exception ex) when (ex.Message.Contains("unique") || ex.Message.Contains("circular"))
        {
            return Conflict(ApiResponse<object>.CreateError(ex.Message, 409));
        }
    }

    [HttpPut("{id}")]
    [HasPermission("TrainingLevel.Update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTrainingLevelCommand command)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse<object>.CreateError("ID mismatch", 400));

        try
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<TrainingLevelDto>.CreateSuccess(result));
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(ApiResponse<object>.CreateError(ex.Message, 404));
        }
        catch (Exception ex) when (ex.Message.Contains("unique") || ex.Message.Contains("circular"))
        {
            return Conflict(ApiResponse<object>.CreateError(ex.Message, 409));
        }
    }

    [HttpDelete("{id}")]
    [HasPermission("TrainingLevel.Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteTrainingLevelCommand(id));
        
        if (!result)
            return NotFound(ApiResponse<object>.CreateError("Training level not found", 404));

        return NoContent();
    }

    [HttpPost("reorder")]
    [HasPermission("TrainingLevel.Update")]
    public async Task<IActionResult> Reorder([FromBody] List<ReorderLevelsDto> levels)
    {
        var command = new ReorderLevelsCommand(levels);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<IEnumerable<TrainingLevelDto>>.CreateSuccess(result));
    }
}
