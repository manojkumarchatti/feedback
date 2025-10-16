# Front-End Implementation Plan (React)

## Tech Stack

- **React 18** with Vite for fast development and TypeScript typing.
- **UI Library**: Chakra UI or Material UI for accessible, responsive components.
- **State Management**: React Query for server state (API data) and Zustand or Context API for UI state.
- **Authentication**: `@azure/msal-react` for handling login and token acquisition.
- **Form Handling**: React Hook Form + Zod for validation.
- **Charting**: Recharts or Chart.js for analytics dashboards.

## Application Structure

```
src/
  app/
    App.tsx
    routes.tsx
    providers/
      AuthProvider.tsx
      QueryProvider.tsx
  features/
    auth/
    feedback/
    analytics/
    admin/
  components/
    layout/
    ui/
  hooks/
  lib/
  types/
```

- `app/providers` configures MSAL, React Query, and routing.
- `features` contains domain modules with slice-based structure (components, api, hooks, types).
- Use [Feature-Sliced Design](https://feature-sliced.design/) principles to keep modules isolated.

## Key Screens

1. **Login Redirect**: After MSAL login, redirect to dashboard.
2. **Employee Dashboard**
   - Quick actions: submit new feedback, view recent responses.
   - Filter by topic/status.
3. **Feedback Submission Flow**
   - Multi-step form capturing category, title, description, anonymity preference, attachments.
   - Client-side validation and autosave drafts (local storage).
4. **Manager Inbox**
   - Table of assigned feedback with SLA indicators.
   - Detail panel with conversation thread and status updates.
5. **Analytics Dashboard (Admin)**
   - Charts (trend lines, heat maps) using aggregated data from `/analytics` endpoints.
   - Export to CSV/PDF options.
6. **Profile & Settings**
   - Display user info from Microsoft Graph (photo, department).
   - Manage notification preferences.

## Authentication Flow

1. Configure MSAL with Entra ID client ID, authority, redirect URIs.
2. Wrap `App` with `MsalProvider` and create `AuthenticatedTemplate` components for protected routes.
3. Acquire access tokens with `acquireTokenSilent` using the API scope before API calls.
4. Attach bearer token in Axios/Fetch interceptors.

## API Integration

- Create API layer with Axios instance configured for base URL and interceptors.
- Define hooks with React Query (`useQuery`, `useMutation`) for data fetching.
- Handle optimistic updates (e.g., marking feedback as resolved).
- Centralize error handling with toast notifications and MSAL re-authentication triggers.

## Testing Strategy

- Unit tests with Vitest + Testing Library for components.
- End-to-end tests with Playwright covering SSO (use MSAL test account or mocked auth).
- Accessibility audits with Axe.

## Performance & Accessibility

- Lazy load feature routes via React Router `lazy` and `Suspense`.
- Implement skeleton loaders for data-heavy views.
- Ensure keyboard navigation and screen reader support (ARIA labels, focus management).
- Optimize bundle with code splitting, tree-shaking, and image compression.

## Local Development

1. Clone the repository and install dependencies (`npm install`).
2. Create `.env.local` with `VITE_AAD_CLIENT_ID`, `VITE_AAD_TENANT_ID`, `VITE_API_BASE_URL`.
3. Run `npm run dev` (Vite defaults to port 5173).
4. Use mock API responses or run the .NET API locally with CORS enabled.

