# AspNetCore-WebApi-CleanArchitecture  

This repository shows clean architecture in practice in a ASP .NET Core Api Project.  

See detailed Information regarding clean architecture concept in the Wiki:  
[Wiki Clean Architecture](https://github.com/AndiHahn/AspNetCore-WebApi-CleanArchitecture/wiki)  

## Technology Stack  
- ASP.NET Core (5.0)  
- Entity Framework Core (5.0.2)  
- Swagger (Swashbuckle.AspNetCore 5.5.1)  
- Azure Blob Storage  

## Project Setup
- Start "CleanArchitecture.Web" Project (localDB CleanArchitectureDb and CleanArchitectureIdentityDb with seed data will be created automatically)  
- Api can be used via swagger documentation on "https://localhost:5001/swagger/index.html"  

### Authentication  
Jwt Authentication is used.  
Token is provided on endpoint /api/User/authenticate with Username: 'user@email.at', Password: 'password'.  
Token can be set globally in swagger "Bearer << token >>"