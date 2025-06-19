using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductsService.Data;
using ProductsService.Repositories;
using ProductsService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// قراءة إعدادات JWT من appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "MyVeryLongSecretKeyForJWTTokenGeneration123456789";

// 🔐 إعداد المصادقة JWT
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

// إضافة الخدمات
builder.Services.AddControllers();

// إضافة Entity Framework
builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(); // للتطوير فقط
});

// Repository Pattern
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Products Service API",
        Version = "v1",
        Description = "خدمة إدارة المنتجات - نظام التجارة الإلكترونية الموزع"
    });
});

var app = builder.Build();

// تهيئة قاعدة البيانات
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    ProductsService.Data.DatabaseSeeder.SeedDatabase(context);
}

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Service API v1");
        c.RoutePrefix = string.Empty; // Swagger على الصفحة الرئيسية
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// ✅ المصادقة والتفويض
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health endpoint
app.MapGet("/health", () => new
{
    Status = "Healthy",
    Service = "Products Service",
    Timestamp = DateTime.UtcNow
});

app.Run();
