Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Building PSP Solution..." -ForegroundColor Green
Write-Host "==================================" -ForegroundColor Cyan

dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Restore failed." -ForegroundColor Red
    exit 1
}

dotnet build --no-restore

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully." -ForegroundColor Green
}
else {
    Write-Host "Build failed." -ForegroundColor Red
}
