using Feedback.Api.Data;
using Feedback.Api.DTOs;
using Feedback.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Api.Services;

public class FeedbackService : IFeedbackService
{
    private readonly AppDbContext _dbContext;

    public FeedbackService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<FeedbackSummaryDto>> GetFeedbackAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.FeedbackEntries
            .Include(entry => entry.Topic)
            .Include(entry => entry.Employee)
            .OrderByDescending(entry => entry.CreatedAt)
            .Select(entry => new FeedbackSummaryDto(
                entry.Id,
                entry.Title,
                entry.Topic!.Name,
                entry.Status,
                entry.CreatedAt,
                entry.Employee!.DisplayName))
            .ToListAsync(cancellationToken);
    }

    public async Task<FeedbackDetailDto?> GetFeedbackAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.FeedbackEntries
            .Include(entry => entry.Topic)
            .Include(entry => entry.Employee)
            .Where(entry => entry.Id == id)
            .Select(entry => new FeedbackDetailDto(
                entry.Id,
                entry.Title,
                entry.Message,
                entry.Status,
                entry.CreatedAt,
                entry.UpdatedAt,
                entry.Topic!.Name,
                entry.Employee!.DisplayName,
                entry.Employee.Email))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<FeedbackDetailDto> CreateFeedbackAsync(string userObjectId, string displayName, string email, CreateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.AzureAdObjectId == userObjectId, cancellationToken);

        if (employee is null)
        {
            employee = new Employee
            {
                AzureAdObjectId = userObjectId,
                DisplayName = displayName,
                Email = email
            };
            _dbContext.Employees.Add(employee);
        }

        var feedback = new FeedbackEntry
        {
            Title = request.Title,
            Message = request.Message,
            TopicId = request.TopicId,
            Employee = employee
        };

        _dbContext.FeedbackEntries.Add(feedback);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _dbContext.Entry(feedback).Reference(f => f.Topic).LoadAsync(cancellationToken);

        return new FeedbackDetailDto(
            feedback.Id,
            feedback.Title,
            feedback.Message,
            feedback.Status,
            feedback.CreatedAt,
            feedback.UpdatedAt,
            feedback.Topic?.Name ?? string.Empty,
            employee.DisplayName,
            employee.Email);
    }

    public async Task<bool> UpdateStatusAsync(int id, FeedbackStatus status, CancellationToken cancellationToken)
    {
        var feedback = await _dbContext.FeedbackEntries.FindAsync(new object[] { id }, cancellationToken);
        if (feedback is null)
        {
            return false;
        }

        feedback.Status = status;
        feedback.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IReadOnlyCollection<FeedbackTopic>> GetTopicsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.FeedbackTopics
            .OrderBy(topic => topic.Name)
            .ToListAsync(cancellationToken);
    }
}
