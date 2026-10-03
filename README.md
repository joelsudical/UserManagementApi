# User Management API

A .NET 8 ASP.NET Core API demonstrating user CRUD, request validation, and logging middleware.

## Run

```powershell
dotnet run
```

Use the HTTPS URL printed by the application and the requests in `UserManagementApi.http`.

## Endpoints

| Method | Route | Result |
| --- | --- | --- |
| GET | `/api/users` | List users |
| GET | `/api/users/{id}` | Get one user |
| POST | `/api/users` | Create a user |
| PUT | `/api/users/{id}` | Replace a user's details |
| DELETE | `/api/users/{id}` | Delete a user |

User input requires a 2-100 character name, a valid email address, and an age from 13 to 120. Invalid requests return `400`; duplicate email addresses return `409`; unknown IDs return `404`.

The sample stores users in memory, so data is cleared when the process restarts. It is intended for an assignment demonstration, not production persistence or authentication.