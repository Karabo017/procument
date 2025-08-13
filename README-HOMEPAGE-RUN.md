Quick start (everything on port 3000)

1) Frontend (React)
   - Open PowerShell in procumentplatform\frontend-react
   - npm install
   - npm run build:deploy  (builds and copies assets into backend/VWProcurement.API/wwwroot)

2) Backend (ASP.NET Core)
   - Open PowerShell in procumentplatform\backend\VWProcurement.API
   - dotnet run --urls=http://localhost:3000

3) Browse and test
   - App: http://localhost:3000
   - Swagger (Dev): http://localhost:3000/swagger
   - Health: GET http://localhost:3000/api/health/database
   - Seed:  POST http://localhost:3000/api/seed/sample-data
