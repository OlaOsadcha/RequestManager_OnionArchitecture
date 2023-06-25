# RequestManager_OnionArchitecture
Application for managing users requests/tasks.

## Technologies & Framework used
- ASP.NET Core 6 
- Entity Framework Core 6 
- AutoMapper
- Blazor (Server)
- XUnit
- Moq
- Bootstrap
- JavaScript (TypeScript)
- CSS (SCSS)
## Architecture
- Onion Architecture
  
## Migrations
--migration add

add-migration InitialCreate -context AppDbContext
update-database -context AppDbContext

--migration remove
remove-migration -context AppDbContext
