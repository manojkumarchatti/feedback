using System.Security.Claims;
using Feedback.Api.DTOs;
using Feedback.Api.Models;
using Feedback.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FeedbackSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var results = await _feedbackService.GetFeedbackAsync(cancellationToken);
        return Ok(results);
    }

    [HttpGet("topics")]
    public async Task<ActionResult<IReadOnlyCollection<FeedbackTopic>>> GetTopics(CancellationToken cancellationToken)
    {
        var results = await _feedbackService.GetTopicsAsync(cancellationToken);
        return Ok(results);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FeedbackDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackService.GetFeedbackAsync(id, cancellationToken);
        if (feedback is null)
        {
            return NotFound();
        }

        return Ok(feedback);
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackDetailDto>> Create([FromBody] CreateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var userObjectId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var displayName = User.FindFirstValue("name") ?? string.Empty;
        var email = User.FindFirstValue(ClaimTypes.Upn) ?? User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userObjectId))
        {
            return Forbid();
        }

        var created = await _feedbackService.CreateFeedbackAsync(userObjectId, displayName, email, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Feedback.Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFeedbackStatusRequest request, CancellationToken cancellationToken)
    {
        var updated = await _feedbackService.UpdateStatusAsync(id, request.Status, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}
