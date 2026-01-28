using FluentValidation;
using Hrms.EmployeeService.Api.Middleware;
using Hrms.EmployeeService.Application.UseCases.CreateEmployee;
using Hrms.EmployeeService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR 14.0.0 - Simplified registration
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly);
});

// FluentValidation 12.1.1
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();

// JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authority = builder.Configuration["Keycloak:Authority"]
      ?? throw new InvalidOperationException("Keycloak Authority missing");

        var audience = builder.Configuration["Keycloak:Audience"]
            ?? throw new InvalidOperationException("Keycloak Audience missing");

        options.Authority = authority;
        options.Audience = audience;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidAudience = audience,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        //  KEYCLOAK ROLE MAPPING FIX
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context?.Principal?.Identity as ClaimsIdentity;

                var resourceAccessClaim = context?.Principal?.FindFirst("resource_access");
                if (resourceAccessClaim != null)
                {
                    using var doc = JsonDocument.Parse(resourceAccessClaim.Value);

                    if (doc.RootElement.TryGetProperty("hrms-api", out var apiElement) &&
                        apiElement.TryGetProperty("roles", out var rolesElement))
                    {
                        foreach (var role in rolesElement.EnumerateArray())
                        {
                            var roleValue = role.GetString();
                            if (!string.IsNullOrEmpty(roleValue))
                            {
                                identity?.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                            }
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };

    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeRead", policy =>
        policy.RequireRole("employee:read", "employee:write"));

    options.AddPolicy("EmployeeWrite", policy =>
        policy.RequireRole("employee:write"));
}); ;

// Controllers
builder.Services.AddControllers();

// Swagger 10.1.0
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Service API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();

public partial class Program { }