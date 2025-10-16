using Feedback.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<FeedbackEntry> FeedbackEntries => Set<FeedbackEntry>();
    public DbSet<FeedbackTopic> FeedbackTopics => Set<FeedbackTopic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().HasIndex(e => e.AzureAdObjectId).IsUnique();

        modelBuilder.Entity<FeedbackEntry>()
            .HasOne(entry => entry.Employee)
            .WithMany(employee => employee.Feedback)
            .HasForeignKey(entry => entry.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FeedbackEntry>()
            .HasOne(entry => entry.Topic)
            .WithMany(topic => topic.Feedback)
            .HasForeignKey(entry => entry.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FeedbackTopic>().HasData(
            new FeedbackTopic { Id = 1, Name = "Culture" },
            new FeedbackTopic { Id = 2, Name = "Career Growth" },
            new FeedbackTopic { Id = 3, Name = "Compensation" },
            new FeedbackTopic { Id = 4, Name = "Work Environment" }
        );
    }
}
