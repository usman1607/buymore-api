using BuyMoreApi.Infrastructure.Extensions;
using BuyMoreApi.Infrastructure.Middlewares;
using BuyMoreApi.Infrastructure.Persistence;
using BuyMoreApi.Application.Validations;
using BuyMoreApi.API.Filters;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BuyMoreApi.Domain.Constants;
using BuyMoreApi.Application.Authentication;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    // Applies validation to all controller endpoints seamlessly
    options.Filters.Add<AutomaticValidationFilter>(); 
});
builder.Services.AddDatabase(configuration);
builder.Services.AddDependencyInjection(configuration);
builder.Services.AddApplicationValidation(); // Register FluentValidation validators

// JWT Authentication
var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(RoleNames.Admin));
    options.AddPolicy("AdminAndCustomer", policy => policy.RequireRole(RoleNames.Customer, RoleNames.Admin));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole(RoleNames.Customer));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole(RoleNames.Staff));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5013")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Buy More API",
        Version = "v1",
        Description = "Buy More API - Multi-tenant AI-powered E-commerce Platform"
    });
 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token"
    });
 
    c.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>()
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await DatabaseInitializer.SeedUserData(app.Services); // Seed user data during development

    // Enable Swagger UI and point it to the JSON path
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Buy Moore API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseExceptionHandling();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();







//DbContext
//Entity Type Configuration
//Migrations

//Exceptions and Global Exception Handling
//Extension Methods
//Middleware and Filters

//Validation and FluentValidation

//Authentication and Authorization - JWT, Identity, Roles, Policies
//Dependency Injection and Service Lifetimes`
 //- Scope, Singleton, Transient

//API Endpoints and Routing

//Policies and Claims-Based Authorization
//Logging and Monitoring
//Mail Services and Notifications
//Payment Integration and External Services
//File Uploads and Storage


//Unit Testing and Integration Testing
//Docker and Containerization
//CI/CD and Deployment






/*
1. DbContext

What it is:
DbContext is the main class in Entity Framework Core that manages database operations. It acts as a bridge between your application and the database.

Responsibilities:

Connects to the database.
Tracks changes made to entities.
Executes queries.
Saves data to the database.

Example:

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}

Simple analogy:
Think of DbContext as a manager that handles all communication between your application and the database.

2. Entity Type Configuration

What it is:
Entity Type Configuration is used to define how a C# entity maps to a database table without cluttering the entity class.

Why use it?

Keeps entity classes clean.
Centralizes database configuration.
Improves maintainability.

Example:

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name)
               .HasMaxLength(100)
               .IsRequired();
    }
}

Simple analogy:
If an entity is a blueprint of a house, Entity Type Configuration defines the building rules and specifications.

3. Exceptions and Global Exception Handling

Exception

An exception is an error that occurs while a program is running.

Example:

int result = 10 / 0; // Throws DivideByZeroException

Global Exception Handling

Instead of handling errors everywhere with try-catch, a central mechanism catches and processes all unhandled exceptions.

Benefits:

Consistent error responses.
Cleaner code.
Centralized logging.

Example:

app.UseExceptionHandler("/error");

Simple analogy:
An exception is like a problem during a journey. Global exception handling is a central customer support desk that handles all reported issues.

4. Extension Methods

What they are:
Extension methods allow you to add new functionality to existing classes without modifying their source code.

Requirements:

Must be in a static class.
Must be a static method.
First parameter uses the this keyword.

Example:

public static class StringExtensions
{
    public static bool IsEmail(this string value)
    {
        return value.Contains("@");
    }
}

Usage:

bool valid = "test@example.com".IsEmail();

Simple analogy:
It's like adding a new tool to a toolbox without changing the toolbox itself.

5. Middleware and Filters
Middleware

What it is:
Middleware is software that sits in the HTTP request pipeline and processes requests and responses.

Responsibilities:

Authentication
Logging
Error handling
Routing

Example:

app.UseAuthentication();
app.UseAuthorization();

Request Flow:

Request → Middleware 1 → Middleware 2 → Controller
Response ← Middleware 1 ← Middleware 2 ← Controller

Simple analogy:
Middleware is like security checkpoints at an airport. Every passenger (request) passes through them before reaching the gate (controller).

Filters

What they are:
Filters run within the MVC/Web API pipeline and allow code to execute before or after controller actions.

Common Types:

Authorization Filters
Action Filters
Exception Filters
Result Filters

Example:

public class LogActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("Action is executing");
    }
}

Simple analogy:
Filters are like assistants assigned specifically to controller actions, performing tasks before or after the action runs.

Quick Comparison
Concept	Purpose
DbContext	Manages database access and entity tracking
Entity Type Configuration	Defines how entities map to database tables
Exceptions	Errors that occur during execution
Global Exception Handling	Centralized handling of application errors
Extension Methods	Add functionality to existing classes without modifying them
Middleware	Processes HTTP requests and responses across the application
Filters	Execute logic before or after controller actions
Middleware vs Filters
Middleware	Filters
Runs for every HTTP request	Runs only for MVC/API actions
Part of HTTP pipeline	Part of MVC pipeline
Used for logging, auth, exception handling	Used for action-specific concerns
Executes before controller selection	Executes around controller actions


*/