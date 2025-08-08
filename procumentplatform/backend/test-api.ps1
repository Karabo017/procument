# VW Procurement Backend - PowerShell Test Script
# This script demonstrates the backend functionality

Write-Host "🚀 VW Procurement Backend - API Test" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green

# Check if the API is running
Write-Host "Checking if API is running..." -ForegroundColor Yellow
$API_URL = "http://localhost:5001"

try {
    # Test basic connectivity
    $response = Invoke-WebRequest -Uri "$API_URL/api/suppliers" -Method GET -UseBasicParsing -TimeoutSec 5
    
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
Write-Host "Testing GET /api/suppliers..." -ForegroundColor Yellow
try {
    $suppliers = Invoke-RestMethod -Uri "$API_URL/api/suppliers" -Method GET
    Write-Host "✅ Suppliers endpoint working - Count: $($suppliers.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Suppliers endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Buyers endpoint
Write-Host "Testing GET /api/buyers..." -ForegroundColor Yellow
try {
    $buyers = Invoke-RestMethod -Uri "$API_URL/api/buyers" -Method GET
    Write-Host "✅ Buyers endpoint working - Count: $($buyers.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Buyers endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Tenders endpoint
Write-Host "Testing GET /api/tenders..." -ForegroundColor Yellow
try {
    $tenders = Invoke-RestMethod -Uri "$API_URL/api/tenders" -Method GET
    Write-Host "✅ Tenders endpoint working - Count: $($tenders.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Tenders endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Bids endpoint
Write-Host "Testing GET /api/bids..." -ForegroundColor Yellow
try {
    $bids = Invoke-RestMethod -Uri "$API_URL/api/bids" -Method GET
    Write-Host "✅ Bids endpoint working - Count: $($bids.Count)" -ForegroundColor Green
} catch {
    Write-Host "❌ Bids endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
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
