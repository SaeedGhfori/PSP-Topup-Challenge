# C4 Model - Component Diagram

## Overview

Each microservice follows Clean Architecture.

The service is divided into independent layers to separate business logic from infrastructure concerns.

---

# Layers

## API

Responsibilities:

- HTTP Endpoints
- Request Handling
- Response Generation

---

## Application

Responsibilities:

- Use Cases
- CQRS
- Validation
- MediatR

---

## Domain

Responsibilities:

- Business Rules
- Entities
- Value Objects
- Domain Events

---

## Infrastructure

Responsibilities:

- External Services
- HTTP Clients
- RabbitMQ
- Third-party Integrations

---

## Persistence

Responsibilities:

- Entity Framework Core
- SQL Server
- Repository
- Unit of Work

---

# Component View

```
+---------------------------+
|           API             |
+-------------+-------------+
              |
              ▼
+---------------------------+
|       Application         |
+-------------+-------------+
              |
              ▼
+---------------------------+
|          Domain           |
+-------------+-------------+
      |               |
      ▼               ▼
+-----------+   +-------------+
|Persistence|   |Infrastructure|
+-----------+   +-------------+
```

---

# Dependency Rule

```
API
 ↓
Application
 ↓
Domain

Infrastructure ─────► Domain

Persistence ────────► Domain
```

Business rules remain isolated inside the Domain layer.

External dependencies never contain business logic.
