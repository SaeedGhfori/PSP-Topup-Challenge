Write-Host "Cleaning solution..." -ForegroundColor Yellow

dotnet clean

Get-ChildItem -Path .. -Recurse -Directory -Include bin,obj |
Remove-Item -Recurse -Force

Write-Host "Clean completed." -ForegroundColor Green
