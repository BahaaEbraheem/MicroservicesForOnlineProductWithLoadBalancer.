using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersService.Data;
using UsersService.Repositories;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// إضافة JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "MyVeryLongSecretKeyForJWTTokenGeneration123456789";

builder.Services.AddAuthentication(options =>
{
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
        ValidIssuer = jwtSettings["Issuer"] ?? "ECommerceSystem",
        ValidAudience = jwtSettings["Audience"] ?? "ECommerceUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// إضافة Entity Framework
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    // استخدام InMemory Database للتطوير والاختبار
    options.UseInMemoryDatabase("UsersDb");
});

// إضافة Repository Pattern
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// إضافة CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Users Service API",
        Version = "v1",
        Description = "خدمة إدارة المستخدمين والمصادقة - نظام التجارة الإلكترونية الموزع"
    });
});

var app = builder.Build();

// تهيئة قاعدة البيانات
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users Service API v1");
        c.RoutePrefix = string.Empty; // جعل Swagger في الصفحة الرئيسية
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// إضافة Authentication و Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// إضافة endpoint للتحقق من صحة الخدمة
app.MapGet("/health", () => new
{
    Status = "Healthy",
    Service = "Users Service",
    Timestamp = DateTime.UtcNow
});

app.Run();
