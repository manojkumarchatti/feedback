# Deployment & DevOps Strategy

## Environments

1. **Development**: Local machines using Docker Compose for API + MySQL, Vite dev server for front-end.
2. **Staging**: Azure resources mirroring production for QA and UAT.
3. **Production**: Highly available Azure infrastructure with monitoring and alerting.

## Infrastructure Components

- **Azure Static Web Apps** or **Azure Front Door + Storage** for hosting React build.
- **Azure App Service** (Linux) for hosting the ASP.NET Core API.
- **Azure Database for MySQL Flexible Server** for managed database.
- **Azure Key Vault** for secrets.
- **Azure Monitor / Application Insights** for observability.
- **Azure Pipelines or GitHub Actions** for CI/CD.
- **Azure AD (Entra ID)** for identity management.

## CI/CD Workflow

1. **Pull Request Validation**
   - Linting, unit tests for frontend (`npm run test -- --watch=false`).
   - Backend unit/integration tests (`dotnet test`).
   - Docker build validation.
2. **Staging Deployment**
   - Build and deploy front-end artifact to staging Static Web App.
   - Deploy API to staging App Service via `az webapp deploy` or container image.
   - Run database migrations using `dotnet ef database update` against staging.
   - Execute smoke tests (Playwright).
3. **Production Deployment**
   - Require manual approval.
   - Use blue/green deployment for API (deployment slots).
   - Apply database migrations with zero-downtime strategy (back up before apply).
   - Invalidate CDN cache for front-end assets.

## Configuration Management

- Store environment-specific settings in Key Vault (connection strings, client IDs, secrets).
- Use Managed Identity for App Service to access Key Vault.
- Parameterize infrastructure with Bicep or Terraform scripts (recommended).

## Monitoring & Alerting

- Application Insights metrics: server response times, failed requests, dependency tracking.
- Azure Monitor alerts for CPU/Memory thresholds, database performance.
- Log retention policy defined per compliance requirements.

## Backup & Recovery

- Enable automated backups for Azure Database for MySQL.
- Schedule export of critical data to Azure Storage for redundancy.
- Document recovery procedure and test quarterly.

## Security Considerations

- Enforce HTTPS and HSTS across services.
- Use Azure AD Conditional Access and MFA.
- Enable CORS policies restricting allowed origins.
- Regularly review app roles and permissions.

