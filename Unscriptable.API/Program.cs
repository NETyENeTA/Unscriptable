using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Text;
using Unscriptable.Application.Interfaces;
using Unscriptable.Infrastructure.Data;
using Unscriptable.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Подключение к базе данных ---
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// Регистрация IDbConnection для Dapper
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

// Регистрация EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- 2. Регистрация сервисов приложения ---
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<CookieService>();
builder.Services.AddHttpContextAccessor(); // Нужно для доступа к Cookies из сервисов

// --- 3. Настройка Аутентификации (JWT + Cookies) ---
builder.Services.AddAuthentication(options =>
{
    // По умолчанию используем JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "Unscriptable.Auth"; // Имя куки должно совпадать в JwtBearer
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(3);
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourFallbackSecretKeyAtLeast32Chars"))
    };

    // КРИТИЧЕСКИЙ БЛОК: Позволяет JWT-аутентификации брать токен из Cookies
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Проверяем наличие куки, если токен не пришел в заголовке Authorization
            if (context.Request.Cookies.ContainsKey("Unscriptable.Auth"))
            {
                context.Token = context.Request.Cookies["Unscriptable.Auth"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 4. Настройка Swagger с поддержкой Bearer ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Unscriptable API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите JWT токен: Bearer {ваш_токен}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    dbContext.Database.EnsureCreated();
}

// --- 5. Конвейер запросов (Middleware) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();