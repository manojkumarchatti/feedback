# System Architecture

## High-Level Overview

The employee feedback platform consists of three primary tiers:

1. **Client (React)**
   - Single-page application built with React, Vite, and TypeScript.
   - Uses Microsoft Authentication Library (MSAL) for Microsoft Entra ID (Azure AD) single sign-on.
   - Communicates with the .NET Web API using HTTPS and JSON.
2. **API (.NET 8)**
   - ASP.NET Core Web API exposing RESTful endpoints for managing feedback, topics, users, and analytics.
   - Implements role-based authorization via Entra ID app roles/role claims.
   - Integrates with Microsoft Graph to enrich user information (optional enhancement).
3. **Data (MySQL 8)**
   - Relational database hosted on Azure Database for MySQL or managed MySQL (e.g., AWS RDS) depending on deployment target.
   - Stores feedback entries, response threads, user metadata, and analytics snapshots.

### Component Diagram

```
+-----------------+       HTTPS        +----------------------+       MySQL Protocol       +-----------------+
| React Front-end | <----------------> | ASP.NET Core Web API | <-----------------------> |   MySQL Server   |
| (MSAL Auth)     |                    | (Azure AD protected) |                          | (Operational DB) |
+-----------------+                    +----------------------+                          +-----------------+
        |                                         ^                                                ^
        v                                         |                                                |
 Microsoft Entra ID (Azure AD) <------------------+                                                |
  (Authentication & Authorization)                                                              Optional ETL
```

## Authentication and Authorization

1. **Azure AD App Registration**
   - Register two applications: one for the SPA client and another for the server API.
   - Configure redirect URIs for local development (`http://localhost:5173`) and production.
   - Expose API scopes (e.g., `api://<api-client-id>/.default` and custom scopes like `Feedback.Read`).
2. **MSAL in React**
   - Use `@azure/msal-browser` and `@azure/msal-react` to manage login, logout, token acquisition, and account state.
   - Request access tokens for the API scope, storing them in memory.
3. **ASP.NET Core Configuration**
   - Configure JWT Bearer authentication using the API app registration.
   - Enforce authorization policies per route (e.g., `Policy("RequireManager")`).
   - Map Entra ID roles/groups to application roles (Employee, Manager, Admin).

## Data Model Summary

| Table              | Purpose                                           |
|--------------------|---------------------------------------------------|
| `users`            | Stores Entra ID object ID, display name, email, role metadata. |
| `feedback_topics`  | Feedback categories/topics for routing purposes.  |
| `feedback_entries` | Core feedback submissions, including sentiment score, tags, status. |
| `feedback_comments`| Threaded manager responses or clarifications.     |
| `attachments`      | Metadata about files stored in blob storage (e.g., Azure Blob Storage). |
| `analytics_cache`  | Aggregated metrics for dashboards (optional).     |

Use Entity Framework Core with migrations to manage schema changes. Apply proper indexing on `feedback_entries` (`created_at`, `topic_id`) for reporting queries.

## Integrations

- **Microsoft Graph (Optional)**: fetch user profile photos, manager relationships.
- **Email/Teams Notifications**: Use Azure Logic Apps or Graph to notify managers when new feedback arrives.
- **Storage**: Use Azure Blob Storage or AWS S3 for file attachments, storing only metadata in MySQL.

## Deployment Considerations

- Host the front-end on Azure Static Web Apps or Azure App Service with CI/CD.
- Deploy the API on Azure App Service or Azure Kubernetes Service.
- Use managed MySQL (Azure Database for MySQL Flexible Server) with VNet integration for security.
- Configure Azure Key Vault for secrets and connection strings.
- Employ Application Insights for logging and monitoring.

