# PSP Topup Service

## Overview

PSP Topup Service is a dedicated microservice responsible for processing mobile top-up transactions.

The service is designed based on Clean Architecture and Domain-Driven Design (DDD) principles and is intended to be consumed by the Payment API Gateway.

This service does not communicate directly with POS devices.

Instead, all requests are received through the Payment Service (API Gateway).

---

## Responsibilities

This service is responsible for:

- Processing Topup requests
- Calling external MCI APIs
- Persisting transaction state
- Maintaining idempotency
- Publishing integration events
- Returning transaction status

---

## System Architecture

```

POS
│
▼
Payment API Gateway
│
▼
PSP.Topup.Api
│
├── SQL Server
│
├── RabbitMQ
│
│
▼
External MCI API

```

---

## Business Flow

1. Payment Service receives a Topup request.
2. Payment validates card transaction.
3. Payment sends request to Topup Service.
4. Topup Service stores transaction.
5. Topup Service calls MCI API.
6. Transaction status is updated.
7. Integration Event is published.
8. Payment Service decides Confirmation or Reversal.

---

## Features

- Clean Architecture
- DDD
- CQRS (MediatR)
- Repository Pattern
- Unit of Work
- Value Objects
- Domain Events
- FluentValidation
- Serilog
- Polly
- RabbitMQ
- SQL Server
- Swagger/OpenAPI
- Docker Ready
- Integration Tests
- Unit Tests

---

## API

POST

```

/api/topups

```

Request

```json
{
  "phoneNumber": "09121234561",
  "amount": 50000,
  "operatorId": 1,
  "idempotencyKey": "REQ-1001"
}
```

Response

```json
{
  "transactionId": "...",
  "status": "TopupSucceeded"
}
```

---

## Idempotency

Each request must contain an IdempotencyKey.

Duplicate requests will return the existing transaction instead of creating a new one.

---

## Transaction Status

- Pending
- TopupSucceeded
- Failed
- ConfirmationSent
- ReversalSent

---

## External Dependencies

### MCI API

Development

```

PSP.Mock.MCI.Api

```

Production

```

Official MCI API

```

Only the BaseUrl changes.

Business logic remains unchanged.

---

## Database

SQL Server

Entity Framework Core

---

## Messaging

RabbitMQ will be used for publishing integration events.

Examples:

- TopupSucceeded
- TopupFailed

---

## Logging

Serilog

Structured Logging

CorrelationId

---

## Future Improvements

- Outbox Pattern
- Inbox Pattern
- Retry Policies
- Circuit Breaker
- Health Checks
- Distributed Tracing
- Metrics
- OpenTelemetry
