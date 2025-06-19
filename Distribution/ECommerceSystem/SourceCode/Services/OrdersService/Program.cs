using Microsoft.EntityFrameworkCore;
using OrdersService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// إضافة Entity Framework
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseInMemoryDatabase("OrdersDb");
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

// تهيئة قاعدة البيانات
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    context.Database.EnsureCreated();
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
