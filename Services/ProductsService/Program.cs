using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Repositories;
using ProductsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// إضافة Entity Framework
builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    // استخدام SQL Server Database
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(); // للتطوير فقط
});

// إضافة Repository Pattern
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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
        Title = "Products Service API",
        Version = "v1",
        Description = "خدمة إدارة المنتجات - نظام التجارة الإلكترونية الموزع"
    });
});

var app = builder.Build();

// تهيئة قاعدة البيانات وإضافة البيانات التجريبية
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    ProductsService.Data.DatabaseSeeder.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Service API v1");
        c.RoutePrefix = string.Empty; // جعل Swagger في الصفحة الرئيسية
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// إضافة endpoint للتحقق من صحة الخدمة
app.MapGet("/health", () => new
{
    Status = "Healthy",
    Service = "Products Service",
    Timestamp = DateTime.UtcNow
});

app.Run();
