# Architecture

## Overview

The PSP Top-up Challenge is designed using a Microservices architecture following Clean Architecture and Domain-Driven Design (DDD) principles.

Each service has a single responsibility and communicates with other services through HTTP and asynchronous messaging.

---

# High-Level Architecture

```
Client (POS)
        │
        ▼
API Gateway (YARP)
        │
        ▼
PSP.Payment.Api
        │
        ├──────────────► PSP.Mock.Bank.Api
        │
        ▼
    RabbitMQ
        │
        ▼
PSP.Topup.Api
        │
        ▼
PSP.Mock.MCI.Api
```

---

# Components

## API Gateway

Responsibilities:

- Single entry point
- Reverse Proxy
- Routing
- Logging

---

## Payment Service

Responsibilities:

- Receive payment requests
- Validate payment
- Call Bank API
- Publish integration events

---

## Top-up Service

Responsibilities:

- Consume payment events
- Call MCI API
- Update recharge status

---

## Mock Bank API

A simulated external banking service used for development and integration testing.

---

## Mock MCI API

A simulated mobile operator service used to emulate recharge operations.

---

# Communication

## Synchronous

- Gateway → Payment
- Payment → Bank
- Topup → MCI

## Asynchronous

- Payment → RabbitMQ
- RabbitMQ → Topup

---

# Architectural Principles

- Microservices
- Clean Architecture
- Domain Driven Design
- CQRS
- Event-Driven Architecture
- Dependency Injection
- Repository Pattern
- Unit of Work
