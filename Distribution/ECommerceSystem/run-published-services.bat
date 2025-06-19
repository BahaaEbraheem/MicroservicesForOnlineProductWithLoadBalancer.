@echo off
echo ========================================
echo Running Published E-Commerce System
echo ========================================
echo.

echo Starting services from Published folder...
echo.

echo Starting Products Service on port 5001...
start "Products Service - Port 5001" cmd /k "cd /d %~dp0Published\ProductsService && ProductsService.exe --urls http://localhost:5001"
timeout /t 4 >nul

echo Starting Users Service on port 5002...
start "Users Service - Port 5002" cmd /k "cd /d %~dp0Published\UsersService && UsersService.exe --urls http://localhost:5002"
timeout /t 4 >nul

echo Starting Orders Service on port 5003...
start "Orders Service - Port 5003" cmd /k "cd /d %~dp0Published\OrdersService && OrdersService.exe --urls http://localhost:5003"
timeout /t 4 >nul

echo Starting Payments Service on port 5004...
start "Payments Service - Port 5004" cmd /k "cd /d %~dp0Published\PaymentsService && PaymentsService.exe --urls http://localhost:5004"
timeout /t 4 >nul

echo Starting API Gateway on port 5000...
start "API Gateway - Port 5000" cmd /k "cd /d %~dp0Published\ApiGateway && ApiGateway.exe --urls http://localhost:5000"
timeout /t 8 >nul

echo.
echo All published services started successfully!
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
echo Published system is ready to use!
echo.
echo Note: These are standalone executables that don't require dotnet CLI
echo To stop: Close all CMD windows
echo.
echo Press any key to exit this window...
pause >nul
