Quick start (port 3000)

1) Build frontend (optional if already built)
   - From procumentplatform/frontend: npm install; npm run build

2) Copy build to backend wwwroot
   - Copy dist/frontend-app/browser/* to procumentplatform/backend/VWProcurement.API/wwwroot/

3) Run backend on port 3000
   - From procumentplatform/backend/VWProcurement.API: dotnet run --urls=http://localhost:3000

4) Open http://localhost:3000
