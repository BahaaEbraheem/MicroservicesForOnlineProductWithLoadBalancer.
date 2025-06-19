@echo off
echo ========================================
echo    تشغيل خدمة الطلبات (Orders Service)
echo ========================================
echo.

cd /d "%~dp0"

echo 🔄 استعادة الحزم...
dotnet restore

echo.
echo 🔄 بناء المشروع...
dotnet build

echo.
echo 🚀 تشغيل خدمة الطلبات...
echo 📍 الخدمة ستعمل على: http://localhost:5003
echo 📍 Swagger UI: http://localhost:5003
echo.
echo ⚠️  للإيقاف اضغط Ctrl+C
echo.

dotnet run --urls "http://localhost:5003"

pause
