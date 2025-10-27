using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Contracts.Validators;
using DatingApp.Data;
using DatingApp.Dtos.User.Login;
using DatingApp.Middlewares;
using DatingApp.Repo;
using DatingApp.Service;
using DatingApp.Service.HelperService;
using DatingApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Serilog --------------------
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// -------------------- CORS --------------------
var corsPolicy = "ApiCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// -------------------- Controllers & JSON --------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// -------------------- FluentValidation --------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// -------------------- Swagger --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DatingApp API",
        Version = "1.0.0",
        Description = "API documentation",
    });

    // JWT Bearer Authentication în Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduceți tokenul JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// -------------------- Health Checks --------------------
builder.Services.AddHealthChecks();

// -------------------- Authentication --------------------
// --- JWT Configuration from Environment Variables ---
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "http://localhost:5098";
var jwtKey = Environment.GetEnvironmentVariable("LOGIN_TOKEN_KEY_PROIECT") ?? "super_secret_key_123456789asdasdasdasd";
var jwtDuration = Environment.GetEnvironmentVariable("JWT_DEFAULT_DURATION") ?? "60"; // fallback 60 min
var jwtDurationMinutes = int.Parse(jwtDuration);

// --- Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// -------------------- DbContext --------------------
builder.Services.AddDbContext<ProiectColectivContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
 b => b.MigrationsAssembly(typeof(ProiectColectivContext).Assembly.GetName().Name)));

// -------------------- Validators --------------------
builder.Services.AddScoped<IValidationFactory, ValidationFactory>();
builder.Services.AddScoped<IRequestValidator, RequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtOptions>>().Value);
builder.Services.AddScoped<ILoginService, LoginService>();

// -------------------- Helper Services --------------------
builder.Services.AddScoped<IPasswordHasherService, PasswordHelperService>();
builder.Services.AddScoped<IAuthorizationHelperService, AuthorizationHelperService>();

// -------------------- Services --------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// -------------------- Repositories / Persistence --------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// -------------------- AutoMapper --------------------
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// -------------------- Build App --------------------
var app = builder.Build();

// -------------------- Middleware --------------------
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingApp API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApplicationExceptionHandler>();

app.MapControllers();
app.MapHealthChecks("/health");

// -------------------- Apply Migrations Automatically --------------------
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ProiectColectivContext>();
//    try
//    {
//        var conn = context.Database.GetDbConnection();
//        Log.Information("Applying migrations with DataSource={DataSource}, Database={Database}", conn.DataSource, conn.Database);
//        context.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        Log.Error(ex, "Automatic migrations failed. DataSource={DataSource}", context.Database.GetDbConnection()?.DataSource);
//        throw;
//    }
//}

// -------------------- Run App --------------------
try
{
    Log.Information("Starting the API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API failed to start!");
}
finally
{
    Log.CloseAndFlush();
}
