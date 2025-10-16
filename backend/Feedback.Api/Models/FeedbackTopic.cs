using System.ComponentModel.DataAnnotations;

namespace Feedback.Api.Models;

public class FeedbackTopic
{
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    public ICollection<FeedbackEntry> Feedback { get; set; } = new List<FeedbackEntry>();
}
