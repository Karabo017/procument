# VW Procurement Backend

A comprehensive ASP.NET Core 8 Web API for managing procurement processes, suppliers, buyers, tenders, and bids in the Volkswagen procurement system.

## 🏗️ Architecture

This project follows Clean Architecture principles with the following layers:

- **VWProcurement.API** - Web API layer with controllers and endpoints
- **VWProcurement.Core** - Domain models, DTOs, and interfaces
- **VWProcurement.Platform** - Business logic and services
- **VWProcurement.Data** - Data access layer with Entity Framework Core

## 🚀 Features

### Core Entities
- **Suppliers** - Manage supplier information and profiles
- **Buyers** - Handle internal buyer accounts and departments
- **Managers** - Manage procurement managers and approvals
- **Tenders** - Create, publish, and manage procurement tenders
- **Bids** - Submit and evaluate supplier bids

### Key Functionality
- ✅ Full CRUD operations for all entities
- ✅ Tender lifecycle management (Draft → Open → Closed → Awarded)
- ✅ Bid submission and evaluation workflow
- ✅ Relationship management between entities
- ✅ Data validation and business rules
- ✅ Repository pattern with Unit of Work
- ✅ Swagger/OpenAPI documentation

## 🛠️ Quick Start

### Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server LocalDB

### Installation

1. **Clone and navigate to the directory**
   ```bash
   cd vw-procurement-backend
   ```

2. **Restore packages and build**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run --project VWProcurement.API
   ```

4. **Access the API**
   - API: `http://localhost:5001`
   - Swagger UI: `http://localhost:5001/swagger`

## 📚 API Endpoints

### Suppliers (`/api/suppliers`)
- `GET /` - Get all suppliers
- `GET /active` - Get active suppliers
- `GET /{id}` - Get supplier by ID
- `POST /` - Create new supplier
- `PUT /{id}` - Update supplier
- `DELETE /{id}` - Delete supplier

### Buyers (`/api/buyers`)
- `GET /` - Get all buyers
- `GET /active` - Get active buyers
- `GET /{id}` - Get buyer by ID
- `POST /` - Create new buyer
- `PUT /{id}` - Update buyer
- `DELETE /{id}` - Delete buyer

### Tenders (`/api/tenders`)
- `GET /` - Get all tenders
- `GET /open` - Get open tenders
- `GET /{id}` - Get tender by ID
- `POST /` - Create new tender
- `PUT /{id}` - Update tender
- `POST /{id}/publish` - Publish tender
- `POST /{id}/close` - Close tender
- `POST /{tenderId}/award/{bidId}` - Award tender

### Bids (`/api/bids`)
- `GET /` - Get all bids
- `GET /{id}` - Get bid by ID
- `GET /by-supplier/{supplierId}` - Get bids by supplier
- `GET /by-tender/{tenderId}` - Get bids by tender
- `POST /submit/{supplierId}` - Submit new bid
- `PUT /{id}` - Update bid
- `DELETE /{id}/withdraw/{supplierId}` - Withdraw bid
- `POST /{id}/review` - Review bid

## 📊 Database Schema

The system uses Entity Framework Core with the following relationships:
- **Suppliers** can submit multiple **Bids**
- **Buyers** can create multiple **Tenders**
- **Managers** can approve **Tenders**
- **Tenders** can receive multiple **Bids**
- Each **Bid** belongs to one **Supplier** and one **Tender**

## 🧪 Testing

Use the Swagger UI at `/swagger` to test all endpoints. Example requests:

**Create Supplier:**
```json
POST /api/suppliers
{
  "name": "ABC Manufacturing",
  "email": "contact@abcmfg.com",
  "phoneNumber": "+1234567890",
  "companyRegistrationNumber": "REG123456"
}
```

**Create Tender:**
```json
POST /api/tenders
{
  "title": "Engine Components Supply",
  "description": "Supply of engine components",
  "estimatedValue": 50000.00,
  "buyerId": 1
}
```

**Submit Bid:**
```json
POST /api/bids/submit/1
{
  "amount": 45000.00,
  "proposal": "Premium components with warranty",
  "tenderId": 1
}
```

## 🔧 Development

### Adding New Features
1. Define models in `VWProcurement.Core/Models`
2. Create DTOs in `VWProcurement.Core/DTOs`
3. Add interfaces in `VWProcurement.Core/Interfaces`
4. Implement repositories in `VWProcurement.Data/Repositories`
5. Create services in `VWProcurement.Platform/Services`
6. Add controllers in `VWProcurement.API/Controllers`

### Project Structure
- **API Layer**: Controllers, middleware, configuration
- **Core Layer**: Models, DTOs, interfaces, enums
- **Platform Layer**: Business logic, services
- **Data Layer**: Entity Framework, repositories, database context

## 🚀 Production Deployment

```bash
# Build for production
dotnet publish -c Release -o ./publish

# Update connection string in appsettings.Production.json
# Deploy to your hosting platform
```

---

**Built for Volkswagen Procurement System** 🚗
