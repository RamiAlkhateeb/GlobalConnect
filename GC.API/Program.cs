using Application.Modules.Admin.Interfaces;
using Application.Modules.Identity.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
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

builder.Services.AddDbContext<GlobalConnectDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLocal"));
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
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

app.Run();
