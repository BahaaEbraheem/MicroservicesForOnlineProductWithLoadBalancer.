var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// إضافة Entity Framework (إذا كان PaymentsService يحتاج قاعدة بيانات)
// builder.Services.AddDbContext<PaymentsDbContext>(options =>
// {
//     // استخدام SQL Server Database
//     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseSqlServer(connectionString);
//     options.EnableSensitiveDataLogging(); // للتطوير فقط
// });

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
        Title = "Payments Service API",
        Version = "v1",
        Description = "خدمة معالجة المدفوعات - نظام التجارة الإلكترونية الموزع"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments Service API v1");
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
    Service = "Payments Service",
    Timestamp = DateTime.UtcNow
});

app.Run();
