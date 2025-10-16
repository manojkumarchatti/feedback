# Employee Feedback Platform
 codex/design-employee-feedback-website-5sqyba
This repository contains a full-stack employee feedback platform that combines a React front-end, .NET Web API back-end, MySQL persistence, and Microsoft Entra ID (Azure AD) single sign-on.

## Project structure

```text
backend/        # .NET 7 Web API project exposing secured feedback endpoints
frontend/       # React + Vite single-page application for employees and managers
docs/           # Architectural and operational documentation
```

## Getting started

### Prerequisites

* Node.js 18+
* .NET 7 SDK
* MySQL 8.x
* Azure AD application registrations for the SPA and the Web API

### Configure Azure AD

1. Create a **Web API** app registration and expose a scope such as `Feedback.ReadWrite`.
2. Create a **single-page application** app registration with redirect URI `http://localhost:5173` and add delegated permissions to the Web API scope.
3. Update the placeholders in `frontend/.env.example` and `backend/Feedback.Api/appsettings.json` with your tenant, client IDs, and scope identifiers. Rename the files to `.env` and `appsettings.Development.json` respectively (or provide secrets via environment variables).

### Set up the database

Create a MySQL database and user that matches the connection string in `appsettings.json`. The API ships with an initial Entity Framework Core migration and will apply it automatically on startup.

```
CREATE DATABASE feedback CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'feedback'@'%' IDENTIFIED BY 'feedback';
GRANT ALL PRIVILEGES ON feedback.* TO 'feedback'@'%';
```

### Run the API

```
cd backend
 dotnet restore
 dotnet run --project Feedback.Api/Feedback.Api.csproj
```

The API listens on `https://localhost:7184` by default and will use the Azure AD configuration to validate bearer tokens.

### Run the front-end

```
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173` in your browser to access the portal.

## Testing the flow

1. Sign in with your work account.
2. Submit new feedback using the form.
3. Review submitted items in the table. Admin users with the `Feedback.Admin` role can update status via the API.

Refer to the documentation in [`docs/`](docs/) for deeper architectural details, deployment guidance, and operational playbooks.
This repository describes the architecture and implementation plan for an employee feedback platform built with React, .NET, MySQL, and Microsoft single sign-on (SSO).

## Overview

The platform enables employees to submit feedback, managers to review and respond, and administrators to analyze engagement trends. It combines a React front-end, a .NET 8 Web API back-end, and a MySQL database, all secured with Microsoft Entra ID (Azure AD) single sign-on.

See the documents in [`docs/`](docs/) for details on architecture, implementation, and deployment.
 main
