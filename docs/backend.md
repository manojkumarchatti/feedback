# Backend Implementation Plan (.NET + MySQL)

## Tech Stack

- **.NET 8** ASP.NET Core Web API.
- **Entity Framework Core** with Pomelo MySQL provider for ORM.
- **MediatR** for request/response separation.
- **FluentValidation** for input validation.
- **Serilog** for structured logging.
- **Docker** for containerized deployment.

## Project Structure

```
src/
  Feedback.Api/
    Controllers/
    Middleware/
    Configuration/
  Feedback.Application/
    Features/
      Feedback/
      Analytics/
      Users/
    Behaviors/
  Feedback.Domain/
    Entities/
    ValueObjects/
    Events/
  Feedback.Infrastructure/
    Persistence/
    Identity/
    Services/
``` 

- `Feedback.Api`: API composition root, DI configuration, controllers.
- `Feedback.Application`: business logic, commands/queries via MediatR.
- `Feedback.Domain`: entity definitions, domain events.
- `Feedback.Infrastructure`: EF Core DbContext, repositories, external service implementations.

## Key Endpoints

| Method | Route | Description | Authorization |
|--------|-------|-------------|---------------|
| GET | `/api/feedback` | List feedback for current user/role. | Employee/Manager |
| POST | `/api/feedback` | Create new feedback entry. | Employee |
| GET | `/api/feedback/{id}` | Fetch details and comments. | Employee (owner), Manager |
| POST | `/api/feedback/{id}/comments` | Add manager response or follow-up. | Manager |
| PATCH | `/api/feedback/{id}/status` | Update status (Open, In Review, Resolved). | Manager/Admin |
| GET | `/api/analytics/summary` | Aggregated metrics for dashboards. | Admin |
| GET | `/api/users/me` | Fetch profile, roles. | Authenticated |

## Authentication & Authorization

- Add `Microsoft.Identity.Web` to simplify Entra ID integration.
- Configure JWT Bearer authentication with `AddMicrosoftIdentityWebApi` using `AzureAd` config section.
- Map Entra ID App Roles to policies:
  - `Employee`
  - `Manager`
  - `Admin`
- Use `User.FindFirst("roles")` for role claims; fallback to group IDs if necessary.

## Data Access

- `ApplicationDbContext` configured with MySQL connection string from configuration/Key Vault.
- Use EF Core migrations: `dotnet ef migrations add InitialCreate`.
- Entities include concurrency tokens (`RowVersion`) to prevent conflicting updates.
- Implement repository interfaces for testing.

### Sample Entity

```csharp
public class FeedbackEntry
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FeedbackTopic Topic { get; set; }
    public FeedbackStatus Status { get; set; } = FeedbackStatus.Open;
    public bool IsAnonymous { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<FeedbackComment> Comments { get; set; } = new();
}
```

## Integration with MySQL

- Use connection pooling and retry policies for transient errors.
- Configure EF Core to run automatic migrations during startup in dev, manual for production.
- Set up database schema with proper indexes (`CreatedAt`, `TopicId`, `Status`).
- Implement background worker to archive old feedback entries.

## Testing Strategy

- Unit tests with xUnit and FluentAssertions.
- Integration tests using `WebApplicationFactory` + Testcontainers for MySQL.
- Contract tests with Pact (optional) to ensure compatibility with frontend expectations.

## Observability

- Use Serilog sinks for console and Application Insights.
- Implement correlation IDs using middleware to link front-end requests.
- Expose `/healthz` (liveness) and `/readyz` (readiness) endpoints.

## Local Development

1. Install .NET 8 SDK.
2. Configure `appsettings.Development.json` with local MySQL connection string.
3. Run `docker compose up` to start MySQL and optional services.
4. Execute migrations `dotnet ef database update`.
5. Run API with `dotnet watch run`.

