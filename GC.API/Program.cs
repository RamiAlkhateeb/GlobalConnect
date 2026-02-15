using Application.Modules.Admin.Interfaces;
using Application.Modules.Identity.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using GlobalConnect.Infrastructure.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// --- 1. GET JWT SETTINGS FROM CONFIG ---
var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "super_secret_key_must_be_long_enough_12345";
var issuer = builder.Configuration["JwtSettings:Issuer"] ?? "GlobalConnectAPI";
var audience = builder.Configuration["JwtSettings:Audience"] ?? "GlobalConnectClient";


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProviderService, ProviderService>();
// Add Services
builder.Services.AddScoped<IAdminService , AdminService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHostedService<TelegramBotService>();

// Configure Authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequest>(); // Finds all validators

//builder.Services.AddDbContext<GlobalConnectDbContext>(options =>
//{
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//    //options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLocal"));
//});
builder.Services.AddDbContext<GlobalConnectDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000" , "http://localhost:5173");
    });
});

// 1. Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:5173"
            )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Important for Auth;

    });
});

var app = builder.Build();

// 2. Use CORS (Must be between UseRouting and UseAuthorization)
app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();


// SEED DATA
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GlobalConnectDbContext>();
    // Auto-migrate (creates .db file if not exists)
    context.Database.Migrate();

    if (!context.Users.Any())
    {
        // Password: "password123" (Hashed with BCrypt)
        string defaultHash = BCrypt.Net.BCrypt.HashPassword("password123");

        var providers = new List<User>
        {
            // 1. Admin User
            new User {
                Name = "Admin",
                Email = "admin@example.com",
                MobileNumber = "0000",
                PasswordHash = defaultHash,
                Role = "Admin",
                IsActive = true
            },

            // 2. Active Provider (Cardiologist)
            new User {
                Name = "Dr. Ahmed Ali",
                Email = "Ahmed@example.com",
                MobileNumber = "01012345678",
                PasswordHash = defaultHash,
                Role = "Provider",
                Specialty = "Cardiologist",
                Bio = "Senior consultant with 15 years experience in interventional cardiology.",
                Nationality = "Egypt",
                GoogleBookingUrl = "https://calendar.google.com",
                IsActive = true,
                PhotoUrl = "https://randomuser.me/api/portraits/men/32.jpg"
            },

            // 3. Active Provider (Dermatologist)
            new User {
                Name = "Dr. Sara Hassan",
                Email = "Sara@example.com",
                MobileNumber = "01112345678",
                PasswordHash = defaultHash,
                Role = "Provider",
                Specialty = "Dermatologist",
                Bio = "Specialist in cosmetic dermatology and laser treatments.",
                Nationality = "Egypt",
                GoogleBookingUrl = "https://calendar.google.com",
                IsActive = true,
                PhotoUrl = "https://randomuser.me/api/portraits/women/44.jpg"
            },

             // 4. Active Provider (Psychiatrist)
            new User {
                Name = "Dr. Omar Khaled",
                Email = "omar@example.com",
                MobileNumber = "01212345678",
                PasswordHash = defaultHash,
                Role = "Provider",
                Specialty = "Psychiatrist",
                Bio = "Helping you achieve mental wellness and balance.",
                Nationality = "Saudi Arabia",
                GoogleBookingUrl = "https://calendar.google.com",
                IsActive = true,
                PhotoUrl = "https://randomuser.me/api/portraits/men/85.jpg"
            },

            // 5. Pending Provider (Inactive - won't show on Home, shows in Admin)
            new User {
                Name = "Dr. New User",
                Email = "newuser@exmaple.com",
                MobileNumber = "01512345678",
                PasswordHash = defaultHash,
                Role = "Provider",
                Specialty = "General Practitioner",
                IsActive = false, // <--- Pending Admin Approval
                PhotoUrl = null
            }
        };

        context.Users.AddRange(providers);
        context.SaveChanges();
    }
}


app.Run();
