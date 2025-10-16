using System.ComponentModel.DataAnnotations;
using Feedback.Api.Models;

namespace Feedback.Api.DTOs;

public record FeedbackSummaryDto(
    int Id,
    string Title,
    string Topic,
    FeedbackStatus Status,
    DateTime CreatedAt,
    string SubmittedBy
);

public record FeedbackDetailDto(
    int Id,
    string Title,
    string Message,
    FeedbackStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string Topic,
    string SubmittedBy,
    string SubmitterEmail
);

public class CreateFeedbackRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public int TopicId { get; set; }
}

public class UpdateFeedbackStatusRequest
{
    [Required]
    public FeedbackStatus Status { get; set; }
}
