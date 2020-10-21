# AspNetCore-WebApi-CleanArchitecture  

This repository shows clean architecture in practice in a ASP .NET Core Api Project.  

## Technology Stack  
- ASP.NET Core (3.1)  
- Entity Framework Core (3.1.1)  
- Swagger (Swashbuckle.AspNetCore 5.5.1)  
- Azure Blob Storage  

## Project Setup
- Start "CleanArchitecture.Web" Project (localDB cleanarchitecturedb with seed data will be created automatically)  
- Api can be used by swagger documentation on "https://localhost:5001/swagger/index.html"  

### Authentication  
Jwt Authentication is used.  
Token is provided on endpoint /api/User/authenticate with Username: username, Password: password.  
Token can be set globally in swagger "Bearer << token >>"

See detailed Information regarding clean architecture concept in the Wiki:
[Wiki Clean Architecture](https://github.com/AndiHahn/AspNetCore-WebApi-CleanArchitecture/wiki)