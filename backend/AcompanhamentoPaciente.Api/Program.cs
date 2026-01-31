using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AcompanhamentoPaciente.Application;
using AcompanhamentoPaciente.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// OpenAPI (Swagger) - only in Development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddOpenApi();
}

// CORS - configurável por ambiente
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AcompanhamentoPaciente.Infrastructure.Data.AppDbContext>();
    try
    {
        AcompanhamentoPaciente.Infrastructure.Data.DbInitializer.Initialize(db);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Database initialization failed");
    }
}

// Development-only middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Production optimizations
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Log startup info
app.Logger.LogInformation("Application started in {Environment} mode", app.Environment.EnvironmentName);
app.Logger.LogInformation("Listening on: {Urls}", string.Join(", ", app.Urls));

app.Run();
