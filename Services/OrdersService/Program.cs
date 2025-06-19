using Microsoft.EntityFrameworkCore;
using OrdersService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// إضافة Entity Framework
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    // استخدام SQL Server Database
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(); // للتطوير فقط
});

// إضافة CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Orders Service API",
        Version = "v1",
        Description = "خدمة إدارة الطلبات - نظام التجارة الإلكترونية الموزع"
    });
});

var app = builder.Build();

// تهيئة قاعدة البيانات وإضافة البيانات التجريبية
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

        Console.WriteLine("🔄 Initializing Orders database...");

        // إنشاء قاعدة البيانات إذا لم تكن موجودة
        var created = context.Database.EnsureCreated();
        if (created)
        {
            Console.WriteLine("✅ Orders database created successfully!");
        }
        else
        {
            Console.WriteLine("ℹ️ Orders database already exists");
        }

        // إضافة البيانات التجريبية
        OrdersService.Data.DatabaseSeeder.SeedDatabase(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error initializing Orders database: {ex.Message}");
        Console.WriteLine($"📋 Stack trace: {ex.StackTrace}");
        throw; // إعادة رمي الاستثناء لإيقاف التطبيق في حالة فشل إنشاء قاعدة البيانات
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders Service API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => new
{
    Status = "Healthy",
    Service = "Orders Service",
    Timestamp = DateTime.UtcNow
});

app.Run();
