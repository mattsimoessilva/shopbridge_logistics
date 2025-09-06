# ShopBridge Logistics Service API

This repository implements the **Logistics Service** of the ShopBridge system, designed as part of the **MVP project for the Third Sprint of the Full Stack Development postgraduate program at CCEC - PUC-Rio**. The service provides a fully functional API for managing customer shipments, enabling creation, retrieval, update, and deletion of shipment data in a microservices architecture.

Developed using **ASP.NET Core**, the service follows a layered architecture and adheres to industry best practices for RESTful APIs. It is designed to be integrated seamlessly into the system orchestration layer via Docker Compose.

---

## Repository Structure

```
shopbridge_logistics/
│
├── Controllers/         # API controllers handling HTTP requests
├── Models/              # Domain models and DTOs
├── Services/            # Business logic and service layer
├── Repositories/        # Data persistence and database access
├── Migrations/          # EF Core database migrations
├── LogisticsApi.csproj      # Project definition
└── README.md
```

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/mattsimoessilva/shopbridge_logistics.git
cd shopbridge_logistics
```

### 2. Configure environment variables
The service expects a `.env` file for database connection strings, ports, and other environment-specific settings. A template `.env.example` is provided.

### 3. Run the service
You can run the service locally via Docker Compose (from the orchestration repository) or directly using `dotnet run`:

```bash
dotnet run --project LogisticsAPI.csproj
```

The API will be available at **http://localhost:5001**.

---

## API Endpoints

### Shipments

| Method | Endpoint           | Description                       |
|--------|------------------|-----------------------------------|
| POST   | `/api/Shipments`     | Creates a new shipment               |
| GET    | `/api/Shipments`     | Retrieves all shipments              |
| GET    | `/api/Shipments/{id}` | Retrieves a specific shipment by ID  |
| PUT    | `/api/Shipments`     | Updates a existing shipment         |
| DELETE | `/api/Shipments/{id}` | Deletes a shipment by ID            |

### Request & Response Schemas

- **ShipmentRequestDTO**: (...)
- **ShipmentResponseDTO**: (...)
- **ProblemDetails**: Standardized error responses.

All endpoints follow REST conventions and return appropriate HTTP status codes (200, 201, 204, 400, 404, 500) with JSON payloads.

---

## Notes

- The service uses **SQLite** for local persistence and supports Docker volumes for data retention.  
- It is designed to operate as part of the **ShopBridge microservices system**, communicating with other services (ProductAPI, OrderAPI) via internal Docker networking.  

---

## References

[1] S. Newman, *Building Microservices: Designing Fine-Grained Systems*. O’Reilly Media, 2015.  
[2] Microsoft, *ASP.NET Core Documentation*, 2025. Available: https://docs.microsoft.com/aspnet/core  
.