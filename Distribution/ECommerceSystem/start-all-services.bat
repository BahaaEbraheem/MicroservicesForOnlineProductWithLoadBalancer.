@echo off
echo ========================================
echo E-Commerce Distributed System Startup
echo ========================================
echo.

echo Building project...
dotnet build ECommerceDistributedSystem.sln
if %errorlevel% neq 0 (
    echo Build failed!
    pause
    exit /b 1
)
echo Build successful!
echo.

echo Starting services...
echo.

echo Starting Products Service on port 5001...
start "Products Service - Port 5001" cmd /k "cd /d %~dp0Services\ProductsService && dotnet run --urls http://localhost:5001"
timeout /t 4 >nul

echo Starting Users Service on port 5002...
start "Users Service - Port 5002" cmd /k "cd /d %~dp0Services\UsersService && dotnet run --urls http://localhost:5002"
timeout /t 4 >nul

echo Starting Orders Service on port 5003...
start "Orders Service - Port 5003" cmd /k "cd /d %~dp0Services\OrdersService && dotnet run --urls http://localhost:5003"
timeout /t 4 >nul

echo Starting Payments Service on port 5004...
start "Payments Service - Port 5004" cmd /k "cd /d %~dp0Services\PaymentsService && dotnet run --urls http://localhost:5004"
timeout /t 4 >nul

echo Starting API Gateway on port 5000...
start "API Gateway - Port 5000" cmd /k "cd /d %~dp0Gateway\ApiGateway && dotnet run --urls http://localhost:5000"
timeout /t 8 >nul

echo.
echo All services started successfully!
echo.
echo Important URLs:
echo    API Gateway: http://localhost:5000
echo    Products Service: http://localhost:5001
echo    Users Service: http://localhost:5002
echo    Orders Service: http://localhost:5003
echo    Payments Service: http://localhost:5004
echo.
echo Swagger UI available on each service
echo Health Check: http://localhost:5000/health
echo.
echo Waiting for services to start...
timeout /t 15 >nul

echo Opening API Gateway in browser...
start http://localhost:5000

echo.
echo System is ready to use!
echo.
echo To test:
echo 1. Open http://localhost:5000 for main page
echo 2. Open http://localhost:5002 for JWT Authentication testing
echo 3. Use Swagger UI on each service
echo.
echo To stop: Close all CMD windows
echo.
echo Press any key to exit this window...
pause >nul
