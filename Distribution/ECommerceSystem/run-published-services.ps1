Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Running Published E-Commerce System" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Starting services from Published folder..." -ForegroundColor Yellow
Write-Host ""

# Start Products Service
Write-Host "Starting Products Service on port 5001..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Published\ProductsService'; Write-Host 'Products Service - Port 5001' -ForegroundColor Green; .\ProductsService.exe --urls 'http://localhost:5001'"
Start-Sleep -Seconds 4

# Start Users Service
Write-Host "Starting Users Service on port 5002..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Published\UsersService'; Write-Host 'Users Service - Port 5002' -ForegroundColor Green; .\UsersService.exe --urls 'http://localhost:5002'"
Start-Sleep -Seconds 4

# Start Orders Service
Write-Host "Starting Orders Service on port 5003..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Published\OrdersService'; Write-Host 'Orders Service - Port 5003' -ForegroundColor Green; .\OrdersService.exe --urls 'http://localhost:5003'"
Start-Sleep -Seconds 4

# Start Payments Service
Write-Host "Starting Payments Service on port 5004..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Published\PaymentsService'; Write-Host 'Payments Service - Port 5004' -ForegroundColor Green; .\PaymentsService.exe --urls 'http://localhost:5004'"
Start-Sleep -Seconds 4

# Start API Gateway
Write-Host "Starting API Gateway on port 5000..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Published\ApiGateway'; Write-Host 'API Gateway - Port 5000' -ForegroundColor Green; .\ApiGateway.exe --urls 'http://localhost:5000'"
Start-Sleep -Seconds 8

Write-Host ""
Write-Host "All published services started successfully!" -ForegroundColor Green
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
Write-Host "Published system is ready to use!" -ForegroundColor Green
Write-Host ""
Write-Host "Note: These are standalone executables that don't require dotnet CLI" -ForegroundColor Cyan
Write-Host "To stop: Close all PowerShell windows" -ForegroundColor Red
Write-Host ""
Write-Host "Press any key to exit this window..." -ForegroundColor Yellow
Read-Host
