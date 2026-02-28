using API.Middleware;
using Application.Interfaces;
using Application.Users.Commands.CreateUser;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// Controllers & Swagger
// ==============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==============================
// CORS
// ==============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ==============================
// Database
// ==============================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")
    )
);

// ==============================
// Repositories
// ==============================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ITrainingLevelRepository, TrainingLevelRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ==============================
// MediatR
// ==============================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly)
);

// ==============================
// JWT Token Generator
// ==============================
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// ==============================
// RBAC Permission Service
// ==============================
builder.Services.AddScoped<IPermissionService, PermissionService>();
// ==============================
// Role Service
// ==============================
builder.Services.AddScoped<RoleService>();

// ==============================
// Authentication (JWT)
// ==============================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSettings:Secret"]!
                )
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// ==============================
// Global Exception Middleware
// ==============================
app.UseMiddleware<GlobalExceptionMiddleware>();

// ==============================
// Swagger (Dev only)
// ==============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==============================
// HTTP Pipeline
// ==============================
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// 🔐 RBAC Permission Middleware
app.UseMiddleware<PermissionMiddleware>();

app.MapControllers();

app.Run();