var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// إضافة HttpClient
builder.Services.AddHttpClient();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "API Gateway - Simple",
        Version = "v1",
        Description = "بوابة API البسيطة - نظام التجارة الإلكترونية"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    c.RoutePrefix = "swagger"; // Swagger متاح على /swagger
});

// تكوين pipeline
app.UseCors("AllowAll");
// app.UseHttpsRedirection(); // معطل لتجنب مشاكل HTTP

app.MapControllers();

// إضافة الصفحة الرئيسية
var homepageContent = @"
<!DOCTYPE html>
<html dir='rtl' lang='ar'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>نظام التجارة الإلكترونية الموزع</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; }
        .container { max-width: 800px; margin: 0 auto; text-align: center; }
        .header { background: rgba(255,255,255,0.1); padding: 30px; border-radius: 15px; margin-bottom: 30px; }
        .services { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin: 30px 0; }
        .service { background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; transition: transform 0.3s; }
        .service:hover { transform: translateY(-5px); }
        .service a { color: white; text-decoration: none; font-weight: bold; }
        .endpoints { background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; margin-top: 20px; text-align: left; }
        .swagger-link { background: rgba(255,255,255,0.2); padding: 15px; border-radius: 10px; margin-top: 20px; }
        .swagger-link a { color: #FFD700; text-decoration: none; font-weight: bold; font-size: 18px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🚀 نظام التجارة الإلكترونية الموزع</h1>
            <p>مرحباً بك في API Gateway - نظام الخدمات المصغرة</p>
            <p>⚡ جميع الخدمات تعمل بنجاح</p>
        </div>

        <div class='services'>
            <div class='service'>
                <h3>📦 خدمة المنتجات</h3>
                <a href='http://localhost:5001' target='_blank'>http://localhost:5001</a>
            </div>
            <div class='service'>
                <h3>👥 خدمة المستخدمين</h3>
                <a href='http://localhost:5002' target='_blank'>http://localhost:5002</a>
            </div>
            <div class='service'>
                <h3>📋 خدمة الطلبات</h3>
                <a href='http://localhost:5003' target='_blank'>http://localhost:5003</a>
            </div>
            <div class='service'>
                <h3>💳 خدمة الدفع</h3>
                <a href='http://localhost:5004' target='_blank'>http://localhost:5004</a>
            </div>
        </div>

        <div class='endpoints'>
            <h3>🔗 النقاط النهائية المتاحة عبر Gateway:</h3>
            <ul>
                <li><strong>GET</strong> /api/products - عرض المنتجات</li>
                <li><strong>POST</strong> /api/users/register - تسجيل مستخدم</li>
                <li><strong>POST</strong> /api/users/login - تسجيل الدخول</li>
                <li><strong>GET</strong> /api/orders - عرض الطلبات</li>
                <li><strong>POST</strong> /api/payments/process - معالجة الدفع</li>
                <li><strong>GET</strong> /health - فحص صحة النظام</li>
            </ul>
        </div>

        <div class='swagger-link'>
            <h3>📋 وثائق API:</h3>
            <a href='/swagger' target='_blank'>🔗 فتح Swagger UI</a>
        </div>
    </div>
</body>
</html>";

app.MapGet("/", () => Results.Content(homepageContent, "text/html"));
app.MapGet("/index.html", () => Results.Content(homepageContent, "text/html"));

// إضافة endpoint للتحقق من صحة Gateway
app.MapGet("/health", () => new
{
    Status = "Healthy",
    Service = "API Gateway",
    Timestamp = DateTime.UtcNow,
    Services = new
    {
        Products = "http://localhost:5001",
        Users = "http://localhost:5002",
        Orders = "http://localhost:5003",
        Payments = "http://localhost:5004"
    }
});

// إضافة proxy endpoints بسيط للخدمات
app.MapGet("/api/products", async (IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.GetStringAsync("http://localhost:5001/api/products");
        return Results.Content(response, "application/json");
    }
    catch (Exception ex)
    {
        return Results.Problem($"خطأ في الاتصال بخدمة المنتجات: {ex.Message}");
    }
});

app.MapPost("/api/users/login", async (IHttpClientFactory httpClientFactory, HttpRequest request) =>
{
    try
    {
        var httpClient = httpClientFactory.CreateClient();
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("http://localhost:5002/api/users/login", content);
        var result = await response.Content.ReadAsStringAsync();
        return Results.Content(result, "application/json");
    }
    catch (Exception ex)
    {
        return Results.Problem($"خطأ في الاتصال بخدمة المستخدمين: {ex.Message}");
    }
});

app.Run();
