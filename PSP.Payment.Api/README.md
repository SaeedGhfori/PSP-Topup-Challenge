# PSP Payment Service

## Overview

PSP Payment Service is the core transaction orchestration service of the PSP platform.

It is responsible for validating purchase requests, communicating with the acquiring bank, coordinating Topup transactions, and managing the complete transaction lifecycle according to PSP standards.

This service acts as the entry point for POS purchase requests and coordinates other microservices using RabbitMQ.

The project is built using Clean Architecture, Domain-Driven Design (DDD), CQRS, and Event-Driven Architecture.

---

## Responsibilities

This service is responsible for:

- Receiving purchase requests from POS devices
- Validating transaction requests
- Creating payment transactions
- Preventing duplicate requests using Idempotency
- Calling Bank Purchase APIs
- Publishing Topup requests
- Consuming Topup completion events
- Sending Confirmation requests
- Sending Reversal requests
- Persisting payment transaction state
- Coordinating the entire payment workflow

---

## System Architecture

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
                                 | Authentication          |
                                 | Routing                 |
                                 | Rate Limiting           |
                                 | Logging                |
                                 +-----------+-------------+
                                             |
                                             |
                                             ▼
                                 +-------------------------+
                                 |    PSP.Payment.Api      |
                                 | Payment Orchestrator    |
                                 +-----------+-------------+
                                             |
                     +-----------------------+-----------------------+
                     |                                               |
                     |                                               |
                     ▼                                               ▼
          +----------------------+                      +----------------------+
          |   Mock Bank API      |                      |      RabbitMQ        |
          | Purchase             |                      | Event Bus            |
          | Confirmation         |                      +----------+-----------+
          | Reversal             |                                 |
          +----------------------+                                 |
                                                                   |
                                                                   ▼
                                                     +--------------------------+
                                                     |     PSP.Topup.Api        |
                                                     | Topup Processing Service |
                                                     +------------+-------------+
                                                                  |
                                                                  |
                                                                  ▼
                                                       +------------------------+
                                                       |     Mock MCI API       |
                                                       | Mobile Recharge        |
                                                       +------------------------+
```

---

## Business Flow

1. POS sends Purchase request.
2. Payment validates request.
3. Payment stores transaction.
4. Payment calls Bank Purchase API.
5. If Purchase succeeds:
   - Publish TopupRequestedIntegrationEvent
6. Topup Service processes the recharge.
7. Topup publishes TopupCompletedIntegrationEvent.
8. Payment consumes the event.
9. Payment sends:
   - Confirmation (Success)
   - Reversal (Failure)
10. Final transaction status is persisted.

---

## Features

- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS
- MediatR
- Repository Pattern
- Unit Of Work
- Value Objects
- Domain Events
- Integration Events
- RabbitMQ
- MassTransit
- Entity Framework Core
- SQL Server
- FluentValidation
- Serilog
- Polly
- OpenAPI
- Scalar API
- Docker Ready
- Idempotency Support

---

## Project Structure

```
PSP.Payment.Api

PSP.Payment.Application

PSP.Payment.Domain

PSP.Payment.Persistence

PSP.Payment.Infrastructure
```

---

## API

### Purchase

POST

```
/api/payments/purchase
```

Request

```json
{
  "pan": "6037991234567890",
  "amount": 50000,
  "phoneNumber": "09121234567",
  "operatorId": 1,
  "terminalId": "12345678",
  "traceNumber": "999999",
  "idempotencyKey": "REQ-1001"
}
```

Response

```json
{
  "transactionId": "8f1b9d92-feba-47a8-bdb9-c5912efaf2f8",
  "status": "Purchased"
}
```

---

## Transaction Status

- Pending
- Purchased
- Confirmed
- Reversed
- Failed

---

## Idempotency

Every purchase request must include a unique IdempotencyKey.

Duplicate requests will return the previously created transaction instead of creating a new one.

---

## Integration Events

Published Events

- TopupRequestedIntegrationEvent

Consumed Events

- TopupCompletedIntegrationEvent

---

## RabbitMQ Flow

```
Payment

↓

TopupRequestedIntegrationEvent

↓

RabbitMQ

↓

Topup Service

↓

TopupCompletedIntegrationEvent

↓

RabbitMQ

↓

Payment Consumer

↓

Confirmation / Reversal
```

---

## External Services

### Bank API

Development

```
PSP.Mock.Bank.Api
```

Production

```
Official Bank Switch
```

---

### Topup Service

Development

```
PSP.Topup.Api
```

Production

```
PSP Topup Service
```

Communication is fully asynchronous through RabbitMQ.

---

## Database

SQL Server

Entity Framework Core

Each microservice owns its own database.

Payment database stores only payment-related data.

---

## Logging

- Serilog
- Structured Logging
- CorrelationId
- Request Tracking

---

## Messaging

RabbitMQ

MassTransit

Event-Driven Communication

Published Events

- TopupRequestedIntegrationEvent

Consumed Events

- TopupCompletedIntegrationEvent

---

## Design Principles

- SOLID
- DRY
- Clean Code
- Separation of Concerns
- Dependency Injection
- Single Responsibility Principle

---

## Future Improvements

- Outbox Pattern
- Inbox Pattern
- Saga Pattern
- Distributed Transactions
- Retry Policies
- Circuit Breaker
- Health Checks
- OpenTelemetry
- Prometheus Metrics
- Distributed Tracing
- API Versioning
- Authentication & Authorization
- Rate Limiting

---

## Technologies

- .NET 9
- ASP.NET Core
- Entity Framework Core
- SQL Server
- RabbitMQ
- MassTransit
- MediatR
- FluentValidation
- Serilog
- Polly
- Scalar
- Docker
