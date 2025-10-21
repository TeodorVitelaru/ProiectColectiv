using DatingApp.Dtos.User.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using DatingApp.Contracts.Services;
using DatingApp.Service;

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
        Version = "v1",
        Description = "Example API documentation"
    });
});

// -------------------- Health Checks --------------------
builder.Services.AddHealthChecks();

// -------------------- Authentication & Authorization --------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.RequireHttpsMetadata = false;
        options.Audience = builder.Configuration["Jwt:Audience"];
    });

builder.Services.AddAuthorization();

// -------------------- LoginService --------------------
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtOptions>>().Value);
builder.Services.AddScoped<ILoginService, LoginService>();

// -------------------- Build app --------------------
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

app.MapControllers();
app.MapHealthChecks("/health");

// -------------------- Run --------------------
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
