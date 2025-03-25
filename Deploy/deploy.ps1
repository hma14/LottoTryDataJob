Write-Host "Building the project..."
$buildResult = dotnet build --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed. Stopping deployment."
    exit $LASTEXITCODE
}


Write-Host "Stopping the LottoTryDataService..."
Stop-Service -Name LottoTryDataService -Force

# Wait until the service is fully stopped
$serviceName = "LottoTryDataService"
$maxWaitTime = 30  # Max wait time in seconds
$waitTime = 0

while ((Get-Service -Name $serviceName).Status -ne "Stopped") {
    Start-Sleep -Seconds 1
    $waitTime++
    if ($waitTime -ge $maxWaitTime) {
        Write-Host "Service did not stop in time, exiting..." -ForegroundColor Red
        exit 1
    }
}

Write-Host "Service stopped, proceeding with deployment..."


Write-Host "Publishing the project..."
$publishResult = dotnet publish "G:\LottoTryDataJob\LottoTryDataJob.csproj" --configuration Release --runtime win-x64 --self-contained false
if ($LASTEXITCODE -ne 0) {
    Write-Host "Publish failed. Stopping deployment."
    exit $LASTEXITCODE
}

Write-Host "Stopping the service if running..."
Stop-Service -Name LottoTryDataService -Force -ErrorAction SilentlyContinue

# Set the deployment destination
$destinationPath = "G:\LottoTryDataJob\Deploy\LottoTryDataService"

# Ensure the destination directory exists
if (!(Test-Path $destinationPath)) {
    Write-Host "Creating deployment directory: $destinationPath"
    New-Item -ItemType Directory -Path $destinationPath | Out-Null
}

Write-Host "Deploying new version..."
Copy-Item -Path "bin\Release\net9.0\win-x64\publish\*" -Destination $destinationPath -Recurse -Force

Write-Host "Starting the service..."

Start-Service -Name LottoTryDataService
Write-Host "Service started successfully!"

Write-Host "Deployment completed successfully!"
