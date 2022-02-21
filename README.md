# AspNetCore-WebApi-CleanArchitecture  

This repository shows clean architecture in practice in a ASP .NET Core Api Project.  

The project includes a modular monolithic application with 2 modules:  
- Shopping module: shows usage of repository and unit of work pattern  
- BudgetPlan module: shows usage of db context with an interface directly in business logic

See detailed Information regarding clean architecture concept in the Wiki:  
[Wiki Clean Architecture](https://github.com/AndiHahn/AspNetCore-WebApi-CleanArchitecture/wiki)  

## Technology Stack  
- ASP.NET Core (6.0)  
- Blazor Server App (6.0)
- Entity Framework Core (6.0.2)  
- Swagger (Swashbuckle.AspNetCore 6.2.3)
- Azure Blob Storage

## Project Setup
- Start "CleanArchitecture.Web.Api" Project (localDB CleanArchitectureDb and CleanArchitectureIdentityDb with seed data will be created automatically)  
- Api can be used via swagger documentation on "https://localhost:5001/swagger/index.html"  

### Authentication  
Jwt Authentication is used.  
Token is provided on endpoint /api/User/authenticate with Username: 'user@email.at', Password: 'password'.  
Token can be set globally in swagger "Bearer << token >>"

The same login can be used in blazor application.