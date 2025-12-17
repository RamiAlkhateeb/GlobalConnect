using Application.Modules.Identity.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using GlobalConnect.Application.Common.Interfaces;
using GlobalConnect.Application.Modules.Availability.Interfaces;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Infrastructure.Data;
using GlobalConnect.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);



// --- 1. GET JWT SETTINGS FROM CONFIG ---
var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "super_secret_key_must_be_long_enough_12345";
var issuer = builder.Configuration["JwtSettings:Issuer"] ?? "GlobalConnectAPI";
var audience = builder.Configuration["JwtSettings:Audience"] ?? "GlobalConnectClient";

// --- 2. ADD AUTHENTICATION SERVICES ---
builder.Services.AddAuthentication(options =>
{
    // These tell the app to use "Bearer" as the default way to check credentials
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequest>(); // Finds all validators

builder.Services.AddDbContext<GlobalConnectDbContext>(options =>
{
    //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLocal"));
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
