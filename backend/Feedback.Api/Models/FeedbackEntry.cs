using System.ComponentModel.DataAnnotations;

namespace Feedback.Api.Models;

public class FeedbackEntry
{
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Title { get; set; }

    [MaxLength(4000)]
    public required string Message { get; set; }

    public FeedbackStatus Status { get; set; } = FeedbackStatus.New;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int TopicId { get; set; }
    public FeedbackTopic? Topic { get; set; }
}

public enum FeedbackStatus
{
    New = 0,
    InReview = 1,
    Resolved = 2,
    Archived = 3
}
