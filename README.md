# PSP Top-up Challenge

A production-oriented microservices solution for processing mobile top-up requests through a simulated PSP (Payment Service Provider).

The project demonstrates modern software architecture principles including **Clean Architecture**, **CQRS**, **DDD**, **Microservices**, **YARP API Gateway**, **Event-Driven Communication**, and integration with external services using dedicated Mock APIs.

---

# Solution Architecture

```
                                    +----------------------+
                                    |        POS           |
                                    | (Payment Terminal)   |
                                    +----------+-----------+
                                               |
                                               |
                                               ▼
                                  +-------------------------+
                                  |       API Gateway       |
                                  |        (YARP)           |
                                  +-----------+-------------+
                                              |
                                              ▼
                                  +-------------------------+
                                  |    PSP.Payment.Api      |
                                  | Payment Orchestrator    |
                                  +-----------+-------------+
                                              |
                     +------------------------+-----------------------+
                     |                                                |
                     ▼                                                ▼
           +----------------------+                       +----------------------+
           |   PSP.Mock.Bank.Api  |                       |      RabbitMQ        |
           |   Bank Simulation    |                       |      Event Bus       |
           +----------------------+                       +----------+-----------+
                                                                    |
                                                                    ▼
                                                      +--------------------------+
                                                      |     PSP.Topup.Api        |
                                                      | Top-up Processing        |
                                                      +------------+-------------+
                                                                   |
                                                                   ▼
                                                       +------------------------+
                                                       |   PSP.Mock.MCI.Api     |
                                                       |  Mobile Operator Mock  |
                                                       +------------------------+
```

---

# Project Structure

```
src
│
├── BuildingBlocks
│   ├── PSP.Contracts
│   ├── PSP.Messaging
│   └── PSP.SharedKernel
│
├── Gateway
│   └── PSP.Gateway.Api
│
├── Mocks
│   ├── PSP.Mock.Bank.Api
│   └── PSP.Mock.MCI.Api
│
└── Services
    ├── Payment
    │   ├── PSP.Payment.Api
    │   ├── PSP.Payment.Application
    │   ├── PSP.Payment.Domain
    │   ├── PSP.Payment.Infrastructure
    │   └── PSP.Payment.Persistence
    │
    └── Topup
        ├── PSP.Topup.Api
        ├── PSP.Topup.Application
        ├── PSP.Topup.Domain
        ├── PSP.Topup.Infrastructure
        └── PSP.Topup.Persistence
```

---

# Technologies

- .NET 10
- ASP.NET Core
- Clean Architecture
- Domain Driven Design (DDD)
- CQRS
- MediatR
- FluentValidation
- Entity Framework Core
- SQL Server
- RabbitMQ
- YARP Reverse Proxy
- Scalar
- Serilog
- Docker

---

# Services

## PSP.Gateway.Api

Single entry point of the system.

Responsibilities:

- Reverse Proxy
- Request Routing
- HTTPS
- Logging

---

## PSP.Payment.Api

Responsible for payment processing.

Responsibilities:

- Receive payment requests
- Validate requests
- Call Bank API
- Publish integration events

---

## PSP.Topup.Api

Responsible for mobile recharge processing.

Responsibilities:

- Consume payment events
- Call MCI API
- Update transaction status

---

## PSP.Mock.Bank.Api

Mock implementation of an external Bank API.

Purpose:

- Simulate payment gateway behavior
- Support multiple payment scenarios
- Integration testing

---

## PSP.Mock.MCI.Api

Mock implementation of an external Mobile Operator API.

Purpose:

- Simulate mobile recharge provider
- Return different recharge statuses
- Integration testing

---

# Request Flow

```
Client

↓

Gateway

↓

Payment Service

↓

Mock Bank

↓

RabbitMQ

↓

Topup Service

↓

Mock MCI
```

---

# Solution Features

- Clean Architecture
- Domain Driven Design
- CQRS
- Repository Pattern
- Unit of Work
- Integration Events
- Event-Driven Architecture
- API Gateway
- Mock External Systems
- Global Exception Handling
- Validation Pipeline
- Structured Logging

---

# Running the Solution

1. Clone repository

```
git clone <repository-url>
```

2. Build solution

```
dotnet build
```

3. Run required services

- SQL Server
- RabbitMQ

4. Run projects

- PSP.Gateway.Api
- PSP.Payment.Api
- PSP.Topup.Api
- PSP.Mock.Bank.Api
- PSP.Mock.MCI.Api

5. Send requests through Gateway.

---

# Documentation

Additional documentation is available under:

```
docs/
```

Including:

- Architecture
- ADR
- Sequence Diagrams
- C4 Model

---

# Current Status

- Solution Structure 
- Gateway 
- Mock Bank 
- Mock MCI 
- Payment Service 
- Top-up Service 
- RabbitMQ 
- Docker 
- Tests 

---

# Notes

This repository was developed as part of a PSP Top-up Challenge.

The external Bank and Mobile Operator services are simulated using dedicated Mock APIs to provide a realistic integration environment without relying on third-party systems.
