Write-Host "========================================" -ForegroundColor Cyan
Write-Host "E-Commerce Distributed System Startup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Build project
Write-Host "Building project..." -ForegroundColor Yellow
dotnet build ECommerceDistributedSystem.sln
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "Build successful!" -ForegroundColor Green
Write-Host ""

Write-Host "Starting services..." -ForegroundColor Yellow
Write-Host ""

# Start Products Service
Write-Host "Starting Products Service on port 5001..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\ProductsService'; Write-Host 'Products Service - Port 5001' -ForegroundColor Green; dotnet run --urls 'http://localhost:5001'"
Start-Sleep -Seconds 4

# Start Users Service
Write-Host "Starting Users Service on port 5002..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\UsersService'; Write-Host 'Users Service - Port 5002' -ForegroundColor Green; dotnet run --urls 'http://localhost:5002'"
Start-Sleep -Seconds 4

# Start Orders Service
Write-Host "Starting Orders Service on port 5003..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\OrdersService'; Write-Host 'Orders Service - Port 5003' -ForegroundColor Green; dotnet run --urls 'http://localhost:5003'"
Start-Sleep -Seconds 4

# Start Payments Service
Write-Host "Starting Payments Service on port 5004..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\PaymentsService'; Write-Host 'Payments Service - Port 5004' -ForegroundColor Green; dotnet run --urls 'http://localhost:5004'"
Start-Sleep -Seconds 4

# Start API Gateway
Write-Host "Starting API Gateway on port 5000..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Gateway\ApiGateway'; Write-Host 'API Gateway - Port 5000' -ForegroundColor Green; dotnet run --urls 'http://localhost:5000'"
Start-Sleep -Seconds 8

Write-Host ""
Write-Host "All services started successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Important URLs:" -ForegroundColor Cyan
Write-Host "   API Gateway: http://localhost:5000" -ForegroundColor White
Write-Host "   Products Service: http://localhost:5001" -ForegroundColor White
Write-Host "   Users Service: http://localhost:5002" -ForegroundColor White
Write-Host "   Orders Service: http://localhost:5003" -ForegroundColor White
Write-Host "   Payments Service: http://localhost:5004" -ForegroundColor White
Write-Host ""
Write-Host "Swagger UI available on each service" -ForegroundColor Yellow
Write-Host "Health Check: http://localhost:5000/health" -ForegroundColor Yellow
Write-Host ""
Write-Host "Waiting for services to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 15

Write-Host "Opening API Gateway in browser..." -ForegroundColor Green
Start-Process "http://localhost:5000"

Write-Host ""
Write-Host "System is ready to use!" -ForegroundColor Green
Write-Host ""
Write-Host "To test:" -ForegroundColor Cyan
Write-Host "1. Open http://localhost:5000 for main page" -ForegroundColor White
Write-Host "2. Open http://localhost:5002 for JWT Authentication testing" -ForegroundColor White
Write-Host "3. Use Swagger UI on each service" -ForegroundColor White
Write-Host ""
Write-Host "To stop: Close all PowerShell windows" -ForegroundColor Red
Write-Host ""
Write-Host "Press any key to exit this window..." -ForegroundColor Yellow
Read-Host
