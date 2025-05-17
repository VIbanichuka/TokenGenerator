# TokenGenerator Api

A secure .NET 8.0 Web API for user registration, authentication, email verification, and 6-digit alphanumeric access token generation and validation.


- **Presentation layer** — [**TokenGenerator.Api**](src/TokenGenerator.Api/)  
  Handles all HTTP requests and responses.

- **Application logic layer** — [**TokenGenerator.Application**](src/TokenGenerator.Application/)  
  Contains the business logic, use case services, DTOs, and interfaces.

- **Domain layer** — [**TokenGenerator.Domain**](src/TokenGenerator.Domain/)  
  Defines core entities like `User` and `Token`

- **Infrastructure & data access layer** — [**TokenGenerator.Infrastructure**](src/TokenGenerator.Infrastructure/)  
  Implements data persistence and external service integrations. Includes EF Core repositories, `DbContext` and MailKit email service.

---

## Features

- ✅ User registration with email confirmation via MailKit
- ✅ Secure login using JWT tokens
- ✅ Email verification with token expiry (max 24h)
- ✅ Generate 6-digit alphanumeric access tokens (valid for up to 3 days)
- ✅ Validate token ownership and expiry
- ✅ Update and delete user accounts securely
- ✅ Follows clean architecture (DTOs, services, repository pattern)
- ✅ Swagger UI with full OpenAPI documentation

---

## Technologies

- **.NET Core Web API 8.0**
- **Entity Framework Core**
- **MailKit** for sending email
- **JWT (System.IdentityModel.Tokens.Jwt)** for authentication
- **AutoMapper**
- **Microsoft SQL Server**
- **Swagger / Swashbuckle**


## Getting Started

### Clone the repo

```bash
gh repo clone VIbanichuka/TokenGenerator


## Sensitive Data Disclaimer

This project **intentionally excludes sensitive credentials and configuration data** for security reasons. The following values have been removed or replaced with placeholders:

- **Database connection string** (`appsettings.json`):
  ```json
  "ConnectionStrings": {
    "TokenGeneratorConnectionString": "server=INSERT YOUR SERVER INSTANCE;database=TokenGeneratorDB;Trusted_Connection=True;TrustServerCertificate=True"
  }

Email configuration (MailKitEmailService.cs):

Sender email and password for SMTP authentication:

smtp.AuthenticateAsync("INSERT YOUR EMAIL", "INSERT YOUR APP PASSWORD");

MailboxAddress.Parse(message.From ?? "INSERT YOUR EMAIL")

Please ensure you replace these placeholder values with your actual credentials or via User Secrets for development, and environment variables or a secure secrets store for production use.
