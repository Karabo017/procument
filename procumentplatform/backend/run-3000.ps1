param(
  [string]$Url = "http://localhost:3000"
)
$ErrorActionPreference = 'Continue'
Write-Host "Preparing to run API on $Url..." -ForegroundColor Cyan

# Kill any process bound to port 3000
try {
  $conns = Get-NetTCPConnection -LocalPort 3000 -ErrorAction SilentlyContinue
  if ($conns) {
    $pids = $conns | Select-Object -ExpandProperty OwningProcess -Unique
    foreach ($pid in $pids) {
      try { Write-Host "Stopping PID $pid using port 3000..." -ForegroundColor Yellow; Stop-Process -Id $pid -Force -ErrorAction Stop } catch {}
    }
    Start-Sleep -Seconds 1
  }
} catch {}

# Build
Push-Location "$PSScriptRoot"
Write-Host "Building solution..." -ForegroundColor Yellow
& dotnet build | Write-Output

# Start API
$apiProj = Join-Path $PSScriptRoot 'VWProcurement.API'
$apiDll  = Join-Path $apiProj 'bin/Debug/net8.0/VWProcurement.API.dll'
Write-Host "Starting API..." -ForegroundColor Yellow
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = $Url
$proc = Start-Process -FilePath 'dotnet' -ArgumentList "`"$apiDll`"" -PassThru -WorkingDirectory $apiProj -WindowStyle Hidden
Write-Host "API PID: $($proc.Id)" -ForegroundColor Green

# Wait for health
$ok = $false
for ($i=0; $i -lt 15; $i++) {
  Start-Sleep -Seconds 1
  try {
    $resp = Invoke-WebRequest -UseBasicParsing -Uri "$Url/api/health/database" -TimeoutSec 3
    if ($resp.StatusCode -eq 200) { $ok = $true; break }
  } catch {}
}
if (-not $ok) { Write-Host "API didn't respond on $Url in time." -ForegroundColor Red; exit 1 }
Write-Host "API is responding on $Url" -ForegroundColor Green

# Seed sample data
try {
  $seed = Invoke-RestMethod -Uri "$Url/api/seed/sample-data" -Method POST
  Write-Host ("Seed: {0}" -f ($seed | ConvertTo-Json -Compress)) -ForegroundColor Cyan
} catch {
  Write-Host "Seed failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Show tenders
try {
  $tenders = Invoke-RestMethod -Uri "$Url/api/tenders" -Method GET
  $count = if ($tenders.count) { $tenders.count } elseif ($tenders.data) { $tenders.data.Count } else { 0 }
  Write-Host "Tenders count: $count" -ForegroundColor Green
} catch {
  Write-Host "Tenders fetch failed: $($_.Exception.Message)" -ForegroundColor Red
}

Pop-Location
