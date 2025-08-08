#!/bin/bash

# VW Procurement Backend - Quick Test Script
# This script demonstrates the backend functionality

echo "🚀 VW Procurement Backend - API Test"
echo "======================================"

# Check if the API is running
echo "Checking if API is running..."
API_URL="http://localhost:5001"

# Test basic connectivity
curl -s -o /dev/null -w "%{http_code}" $API_URL/api/suppliers > response_code.txt
RESPONSE_CODE=$(cat response_code.txt)

if [ $RESPONSE_CODE -eq 200 ]; then
    echo "✅ API is running successfully on $API_URL"
else
    echo "❌ API is not accessible. Please start the application first:"
    echo "   dotnet run --project VWProcurement.API"
    exit 1
fi

echo ""
echo "📊 Testing API Endpoints:"
echo "========================"

# Test Suppliers endpoint
echo "Testing GET /api/suppliers..."
curl -s "$API_URL/api/suppliers" | head -c 100
echo "..."

echo ""
echo "Testing GET /api/buyers..."
curl -s "$API_URL/api/buyers" | head -c 100
echo "..."

echo ""
echo "Testing GET /api/tenders..."
curl -s "$API_URL/api/tenders" | head -c 100
echo "..."

echo ""
echo "Testing GET /api/bids..."
curl -s "$API_URL/api/bids" | head -c 100
echo "..."

echo ""
echo "🎉 Backend API is functional!"
echo "Visit $API_URL/swagger for interactive API documentation"

# Cleanup
rm -f response_code.txt
