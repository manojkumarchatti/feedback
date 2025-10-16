using System.ComponentModel.DataAnnotations;

namespace Feedback.Api.Models;

public class Employee
{
    public int Id { get; set; }

    [MaxLength(200)]
    public required string DisplayName { get; set; }

    [MaxLength(200)]
    public required string Email { get; set; }

    [MaxLength(200)]
    public required string AzureAdObjectId { get; set; }

    public ICollection<FeedbackEntry> Feedback { get; set; } = new List<FeedbackEntry>();
}
