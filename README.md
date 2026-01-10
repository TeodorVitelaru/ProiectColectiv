# DatingApp - Documentație Tehnică

## Prezentare Generală

Aplicație backend ASP.NET Core 9.0 pentru platformă de dating: autentificare JWT, matchmaking, mesagerie, review-uri, raportări, gestionare imagini.

## Stack Tehnologic

- **.NET 9.0** + **ASP.NET Core Web API**
- **Entity Framework Core 9.0** + **SQL Server**
- **JWT Bearer** + **BCrypt.Net-Next 4.0.3**
- **FluentValidation 11.3.1**, **AutoMapper 12.0.1**, **Serilog 9.0.0**
- **Swagger/OpenAPI**

## Arhitectură

Clean Architecture în layere:

```
Controllers → Services → Repositories → Domain (Entities)
```

Pattern-uri: Repository, Unit of Work, DI, DTO, Factory, Middleware

## Structura Proiectului

### Controllers/

Controlere API pentru request-uri HTTP:

- `AuthController` - Înregistrare utilizatori
- `LoginController` - Autentificare
- `UserController` - CRUD utilizatori
- `MatchController`, `MessageController`, `ReviewController`, `ReportController`, `ImageController`

### Service/

Business logic:

- `LoginService`, `RegisterSimpleService`, `UserService`, `MatchService`, `MessageService`, etc.
- **HelperService/** - `PasswordHelperService` (BCrypt), `AuthorizationHelperService`

### Repo/

Repository layer pentru acces la date:

- `Repository` (generic), `UnitOfWork`
- Repository-uri specifice: `UserRepository`, `MatchRepository`, `MessageRepository`, etc.

### Domain/

**Entities:** `User`, `Match`, `Message`, `Review`, `Report`, `Image`, `UserLanguage`, `UserInterest`
**Primitives:** Clase de bază (`Entity<T>`)

### Dtos/

Data Transfer Objects pentru API: `User/`, `Match/`, `Message/`, `Review/`, `Report/`, `Image/`

### Enums/

`Gender`, `SexualOrientation`, `RelationshipGoal`, `Language`, `Interest`

### Data/

- `ProiectColectivContext` - DbContext EF Core
- **Migrations/** - Migrații bază de date

### Contracts/

Interfețe pentru DI:

- **Persistence/** - `IRepository`, `IUnitOfWork`, repository-uri specifice
- **Services/** - Interfețe pentru servicii
- **Validators/** - `IValidationFactory`, `IRequestValidator`

### Middlewares/

- `ApplicationExceptionHandler` - Exception handling global
- `SqlExceptionHandler` - Excepții SQL

### Validators/

- `RequestValidator`, `ValidationFactory` - FluentValidation

### Mapper/

- `MappingProfile` - Configurări AutoMapper

## Configurare și Setup

### Instalare

```bash
dotnet restore
dotnet ef database update
```

### Configurare JWT (variabile de mediu)

```bash
# PowerShell
$env:JWT_ISSUER="http://localhost:5098"
$env:LOGIN_TOKEN_KEY_PROIECT="super_secret_key_123456789asdasdasdasd"
$env:JWT_DEFAULT_DURATION="60"
```

Sau în `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DatingAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "http://localhost:5098",
    "Key": "super_secret_key_123456789asdasdasdasd",
    "DefaultDuration": "60"
  }
}
```

### Rulare

```bash
dotnet run                              # Development
dotnet run --configuration Release      # Production
```

**Swagger UI:** `http://localhost:5098/`

## Baza de Date

### Entități Principale

| Entitate          | Câmpuri                                                                                                                                                  |
| ----------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Users**         | id, firstName, lastName, email, password, age, height, gender, city, bio, relationshipGoal, sexualOrientation, preferredAgeMin, preferredAgeMax, isAdmin |
| **Matches**       | id, user1Id, user2Id, matchedAt                                                                                                                          |
| **Messages**      | id, senderId, receiverId, content, sentAt                                                                                                                |
| **Reviews**       | id, reviewerId, reviewedUserId, rating, comment                                                                                                          |
| **Reports**       | id, reporterId, reportedUserId, reason, description                                                                                                      |
| **Images**        | id, userId, imageData (VARBINARY), contentType                                                                                                           |
| **UserLanguages** | userId, language (many-to-many)                                                                                                                          |
| **UserInterests** | userId, interest (many-to-many)                                                                                                                          |

### Migrații

```bash
dotnet ef migrations add <NomeMigratie>
dotnet ef database update
dotnet ef migrations remove              # Șterge ultima migrație
```

**EF Core:** Code-First, Migrații în `Data/Migrations/`, Fluent API în `Repo/Configurations/`

## Autentificare JWT

### Flow

1. **Înregistrare:** `POST /api/auth/register` → returnează JWT token
2. **Login:** `POST /api/login` → validează credențiale și returnează JWT token
3. **Request autentificat:** Header `Authorization: Bearer <token>`

### Configurare Token

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
```

### Protejare Endpoint-uri

```csharp
[Authorize]                 // Necesită autentificare
[AllowAnonymous]            // Permite acces neautentificat
```

## API Endpoints

| Method          | Endpoint             | Descriere                    | Auth |
| --------------- | -------------------- | ---------------------------- | ---- |
| POST            | `/api/auth/register` | Înregistrare user            | No   |
| POST            | `/api/login`         | Autentificare                | No   |
| GET             | `/api/users`         | Listare utilizatori          | Yes  |
| GET/PUT/DELETE  | `/api/users/{id}`    | Detalii/Update/Ștergere user | Yes  |
| GET/POST/DELETE | `/api/matches`       | Match-uri                    | Yes  |
| GET/POST        | `/api/messages`      | Mesaje                       | Yes  |
| GET/POST/DELETE | `/api/reviews`       | Review-uri                   | Yes  |
| POST/GET        | `/api/reports`       | Raportări                    | Yes  |
| POST/GET/DELETE | `/api/images`        | Imagini                      | Yes  |
| GET             | `/health`            | Health check                 | No   |

## Dependency Injection

Configurare în `Program.cs`:

```csharp
// Services (Scoped)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMatchService, MatchService>();

// Repositories (Scoped)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Validators & Helpers
builder.Services.AddScoped<IValidationFactory, ValidationFactory>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHelperService>();

// Singleton
builder.Services.AddSingleton<JwtOptions>();
```

## Validare cu FluentValidation

Validarea se execută automat înainte de controller:

```csharp
public class RegisterUserValidator : AbstractValidator<RegisterSimpleUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Age).InclusiveBetween(18, 100);
    }
}
```

## Logging cu Serilog

Configurare în `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": { "Default": "Information" },
    "WriteTo": [{ "Name": "Console" }]
  }
}
```

Utilizare:

```csharp
_logger.LogInformation("Getting user with ID {UserId}", id);
```

**Nivele:** Trace, Debug, Information, Warning, Error, Fatal

## Exception Handling

Middleware centralizat (`ApplicationExceptionHandler`):

- `NotFoundException` → 404
- `BadRequestException` → 400
- `ValidationException` → 400 cu detalii
- `Exception` → 500

Format răspuns:

```json
{
  "error": "Resource not found",
  "details": "User with ID 123 not found",
  "timestamp": "2026-01-10T10:30:00Z"
}
```

## Best Practices

### Securitate

- Schimbă `JWT_KEY` în producție
- Folosește HTTPS în producție
- Restricționează CORS (`AllowAnyOrigin` doar în dev)
- Nu loghează parole sau date sensibile

### Baza de Date

- Folosește migrații pentru schema changes
- Testează migrațiile înainte de deployment
- Backup-uri regulate

### Performance

- Folosește async/await pentru I/O
- Optimizează query-uri EF Core (Include, Select)
- Evită N+1 problem (query-uri în loop-uri)

## Resurse

- **Swagger UI:** `http://localhost:5098/`
- **Health Check:** `http://localhost:5098/health`
- **Testing HTML:** `http://localhost:5098/register-test.html`
- **ASP.NET Core Docs:** https://docs.microsoft.com/aspnet/core
- **EF Core Docs:** https://docs.microsoft.com/ef/core

---

**Versiune:** 1.0.0 | **Framework:** .NET 9.0 | **Ultima actualizare:** Ianuarie 2026
