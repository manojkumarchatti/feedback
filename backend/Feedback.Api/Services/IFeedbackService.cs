using Feedback.Api.DTOs;
using Feedback.Api.Models;

namespace Feedback.Api.Services;

public interface IFeedbackService
{
    Task<IReadOnlyCollection<FeedbackSummaryDto>> GetFeedbackAsync(CancellationToken cancellationToken);
    Task<FeedbackDetailDto?> GetFeedbackAsync(int id, CancellationToken cancellationToken);
    Task<FeedbackDetailDto> CreateFeedbackAsync(string userObjectId, string displayName, string email, CreateFeedbackRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateStatusAsync(int id, FeedbackStatus status, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<FeedbackTopic>> GetTopicsAsync(CancellationToken cancellationToken);
}
