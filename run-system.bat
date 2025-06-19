@echo off
chcp 65001 >nul
echo ========================================
echo Starting E-Commerce Distributed System
echo ========================================
echo.

echo What will happen:
echo - Check if databases exist
echo - Create databases if not found
echo - Create tables automatically
echo - Add seed data if not found
echo.

echo Make sure:
echo - SQL Server is running
echo - Connection String is correct in appsettings.json
echo - You have permissions to create databases
echo.

echo Starting API Gateway (Port 5000)...
start "API Gateway" cmd /k "chcp 65001 >nul && cd Gateway\ApiGateway && dotnet run --urls http://localhost:5000"

echo Starting Users Service (Port 5002)...
start "Users Service" cmd /k "chcp 65001 >nul && cd Services\UsersService && dotnet run --urls http://localhost:5002"

echo Starting Products Service (Port 5001)...
start "Products Service" cmd /k "chcp 65001 >nul && cd Services\ProductsService && dotnet run --urls http://localhost:5001"

echo Starting Orders Service (Port 5003)...
start "Orders Service" cmd /k "chcp 65001 >nul && cd Services\OrdersService && dotnet run --urls http://localhost:5003"

echo Starting Payments Service (Port 5004)...
start "Payments Service" cmd /k "chcp 65001 >nul && cd Services\PaymentsService && dotnet run --urls http://localhost:5004"

echo.
echo All services started in separate windows
echo.
echo Watch the windows for these messages:
echo "Database created for the first time" (if new)
echo "Database already exists" (if exists)
echo "Added X users/products/orders" (seed data)
echo.
echo Important URLs:
echo - API Gateway: http://localhost:5000
echo - Products Service: http://localhost:5001
echo - Users Service: http://localhost:5002
echo - Orders Service: http://localhost:5003
echo - Payments Service: http://localhost:5004
echo.
echo Swagger UIs:
echo - API Gateway: http://localhost:5000/swagger
echo - Products: http://localhost:5001/swagger
echo - Users: http://localhost:5002/swagger
echo - Orders: http://localhost:5003/swagger
echo - Payments: http://localhost:5004/swagger
echo.
echo Wait for initialization messages in all windows
echo.
echo Waiting 10 seconds for services to start...
timeout /t 10 /nobreak >nul

echo Opening API Gateway in browser...
start http://localhost:5000

echo.
echo System is ready! All services are running.
echo.
pause
