# تعيين ترميز UTF-8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Starting E-Commerce Distributed System" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "What will happen:" -ForegroundColor Yellow
Write-Host "- Check if databases exist" -ForegroundColor White
Write-Host "- Create databases if not found" -ForegroundColor White
Write-Host "- Create tables automatically" -ForegroundColor White
Write-Host "- Add seed data if not found" -ForegroundColor White
Write-Host ""

Write-Host "Make sure:" -ForegroundColor Yellow
Write-Host "- SQL Server is running" -ForegroundColor White
Write-Host "- Connection String is correct in appsettings.json" -ForegroundColor White
Write-Host "- You have permissions to create databases" -ForegroundColor White
Write-Host ""

Write-Host "Starting API Gateway (Port 5000)..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Gateway\ApiGateway'; Write-Host 'API Gateway - Port 5000' -ForegroundColor Green; dotnet run --urls 'http://localhost:5000'"

Write-Host "Starting Users Service (Port 5002)..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\UsersService'; Write-Host 'Users Service - Port 5002' -ForegroundColor Green; dotnet run --urls 'http://localhost:5002'"

Write-Host "Starting Products Service (Port 5001)..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\ProductsService'; Write-Host 'Products Service - Port 5001' -ForegroundColor Green; dotnet run --urls 'http://localhost:5001'"

Write-Host "Starting Orders Service (Port 5003)..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\OrdersService'; Write-Host 'Orders Service - Port 5003' -ForegroundColor Green; dotnet run --urls 'http://localhost:5003'"

Write-Host "Starting Payments Service (Port 5004)..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'Services\PaymentsService'; Write-Host 'Payments Service - Port 5004' -ForegroundColor Green; dotnet run --urls 'http://localhost:5004'"

Write-Host ""
Write-Host "All services started in separate windows" -ForegroundColor Green
Write-Host ""
Write-Host "Watch the windows for these messages:" -ForegroundColor Yellow
Write-Host '"Database created for the first time" (if new)' -ForegroundColor White
Write-Host '"Database already exists" (if exists)' -ForegroundColor White
Write-Host '"Added X users/products/orders" (seed data)' -ForegroundColor White
Write-Host ""
Write-Host "Important URLs:" -ForegroundColor Yellow
Write-Host "- API Gateway: http://localhost:5000" -ForegroundColor White
Write-Host "- Products Service: http://localhost:5001" -ForegroundColor White
Write-Host "- Users Service: http://localhost:5002" -ForegroundColor White
Write-Host "- Orders Service: http://localhost:5003" -ForegroundColor White
Write-Host "- Payments Service: http://localhost:5004" -ForegroundColor White
Write-Host ""
Write-Host "Swagger UIs:" -ForegroundColor Yellow
Write-Host "- API Gateway: http://localhost:5000/swagger" -ForegroundColor White
Write-Host "- Products: http://localhost:5001/swagger" -ForegroundColor White
Write-Host "- Users: http://localhost:5002/swagger" -ForegroundColor White
Write-Host "- Orders: http://localhost:5003/swagger" -ForegroundColor White
Write-Host "- Payments: http://localhost:5004/swagger" -ForegroundColor White
Write-Host ""
Write-Host "Wait for initialization messages in all windows" -ForegroundColor Yellow
Write-Host ""
Write-Host "Waiting 10 seconds for services to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host "Opening API Gateway in browser..." -ForegroundColor Green
Start-Process "http://localhost:5000"

Write-Host ""
Write-Host "System is ready! All services are running." -ForegroundColor Green
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Yellow
Read-Host
