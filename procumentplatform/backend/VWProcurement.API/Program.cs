using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VWProcurement.Core.Interfaces;
using VWProcurement.Data;
using VWProcurement.Data.Repositories;
using VWProcurement.API;

var builder = WebApplication.CreateBuilder(args);

// Configure to listen on port 3000
builder.WebHost.UseUrls("http://localhost:3000");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework (SQL Server)
builder.Services.AddDbContext<VWProcurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register repositories (skip broken Platform services for now)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Note: Seeding is available via /api/seed/sample-data endpoint

// Add JWT Authentication
var jwtSecret = builder.Configuration["JwtSettings:SecretKey"] ?? "vw-procurement-super-secret-jwt-key-for-token-generation-2024";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Enable static files
app.UseDefaultFiles();
app.UseStaticFiles();

// Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Fallback to index.html for client-side routes
app.MapFallbackToFile("index.html");

// Ensure database is created and seed sample data in Development (non-fatal if DB unavailable)
try
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    var context = scope.ServiceProvider.GetRequiredService<VWProcurementDbContext>();
    context.Database.EnsureCreated();
    // Optional: call the seeding endpoint manually if needed
    logger.LogInformation("Database check complete (EnsureCreated). Ready.");
}
catch (Exception ex)
{
    var fallbackLogger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    fallbackLogger.LogWarning(ex, "Database initialization failed. Continuing to serve static files and API endpoints that don't require DB.");
}

app.Run();
