Write-Host "Starting PSP Solution..." -ForegroundColor Green

Start-Process powershell -ArgumentList "dotnet run --project ../src/Gateway/PSP.Gateway.Api"

Start-Process powershell -ArgumentList "dotnet run --project ../src/Mocks/PSP.Mock.Bank.Api"

Start-Process powershell -ArgumentList "dotnet run --project ../src/Mocks/PSP.Mock.MCI.Api"

Start-Process powershell -ArgumentList "dotnet run --project ../src/Services/Payment/PSP.Payment.Api"

Start-Process powershell -ArgumentList "dotnet run --project ../src/Services/Topup/PSP.Topup.Api"

Write-Host "All services started." -ForegroundColor Green
