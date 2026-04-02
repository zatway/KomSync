using Infrastructure;
using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Подключаем слои
// ---------------------------
builder.Services.ConfigureAddApplication();
builder.Services.ConfigureAddInfrastructure(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddScoped<IRealtimeNotificationPublisher, WebApi.Services.SignalRNotificationPublisher>();
builder.Services.AddScoped<IFileStorage, WebApi.Services.LocalFileStorage>();
builder.Services.Configure<WebApi.Services.SeedAdminSettings>(builder.Configuration.GetSection("SeedAdmin"));
builder.Services.AddHostedService<WebApi.Services.SeedAdminHostedService>();

// ---------------------------
// Контроллеры и JSON
// ---------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

// ---------------------------
// Глобальный обработчик ошибок
// ---------------------------
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ---------------------------
// Swagger
// ---------------------------
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KomSync API",
        Version = "v1"
    });

    const string bearerScheme = "Bearer";

    opt.AddSecurityDefinition(bearerScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    opt.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(bearerScheme, document)] = new List<string>()
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",     // React/Vite/Next и т.д.
                "http://localhost:5173",     // Vite default
                "http://localhost:5174",     // Vite (другой порт)
                "http://localhost:4200",     // Angular
                "https://your-frontend-domain.com"   // продакшен потом
            )
            .AllowAnyHeader()           // ← важно для Content-Type, Authorization и т.д.
            .AllowAnyMethod()           // GET, POST, PUT, PATCH, DELETE, OPTIONS
            .AllowCredentials();        // если используешь куки / credentials: 'include'
    });
});

// ---------------------------
// JWT Authentication
// ---------------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"] ?? "Super_Secret_Key_At_Least_32_Chars_Long";
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var path = context.HttpContext.Request.Path;
            if (path.StartsWithSegments("/hubs"))
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                    context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// ---------------------------
// HTTP Context
// ---------------------------
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ---------------------------
// Middleware pipeline
// ---------------------------
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KomSync API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontendDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<WebApi.Hubs.NotificationHub>("/hubs/notifications");

app.Run();