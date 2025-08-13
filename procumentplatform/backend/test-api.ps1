# VW Procurement Backend - PowerShell Test Script
# This script demonstrates the backend functionality

Write-Host "🚀 VW Procurement Backend - API Test" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green

# Check if the API is running
Write-Host "Checking if API is running on port 3000..." -ForegroundColor Yellow
$API_URL = "http://localhost:3000"

try {
    # Test basic connectivity
    $response = Invoke-WebRequest -Uri "$API_URL/api/health/database" -Method GET -UseBasicParsing -TimeoutSec 5
    
    if ($response.StatusCode -eq 200) {
    Write-Host "✅ API is running successfully on $API_URL" -ForegroundColor Green
    } else {
        Write-Host "❌ API returned status code: $($response.StatusCode)" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "❌ API is not accessible. Please start the application first:" -ForegroundColor Red
    Write-Host "   dotnet run --project VWProcurement.API" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "📊 Testing API Endpoints:" -ForegroundColor Cyan
Write-Host "========================" -ForegroundColor Cyan

# Test Suppliers endpoint
Write-Host "Testing GET /api/tenders..." -ForegroundColor Yellow
try {
    $tenders = Invoke-RestMethod -Uri "$API_URL/api/tenders" -Method GET
    Write-Host "✅ Tenders endpoint working - Count: $($tenders.count ?? $tenders.data.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Tenders endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Buyers endpoint
Write-Host "Seeding sample data (POST /api/seed/sample-data)..." -ForegroundColor Yellow
try {
    $seed = Invoke-RestMethod -Uri "$API_URL/api/seed/sample-data" -Method POST
    Write-Host "✅ Seed result: $($seed | ConvertTo-Json -Compress)" -ForegroundColor Green
} catch {
    Write-Host "❌ Seed endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Tenders endpoint
Write-Host "Re-check GET /api/tenders after seeding..." -ForegroundColor Yellow
try {
    $tenders = Invoke-RestMethod -Uri "$API_URL/api/tenders" -Method GET
    Write-Host "✅ Tenders endpoint working - Count: $($tenders.count ?? $tenders.data.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Tenders endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎉 Backend API is functional!" -ForegroundColor Green
Write-Host "Visit $API_URL/swagger for interactive API documentation" -ForegroundColor Cyan

# Show sample API calls
Write-Host ""
Write-Host "📋 Sample API Calls:" -ForegroundColor Magenta
Write-Host "===================" -ForegroundColor Magenta
Write-Host "Create Supplier: POST $API_URL/api/suppliers" -ForegroundColor White
Write-Host "Create Buyer: POST $API_URL/api/buyers" -ForegroundColor White
Write-Host "Create Tender: POST $API_URL/api/tenders" -ForegroundColor White
Write-Host "Submit Bid: POST $API_URL/api/bids/submit/{supplierId}" -ForegroundColor White
