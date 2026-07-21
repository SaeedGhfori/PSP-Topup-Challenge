# C4 Model - Container Diagram

## Overview

The Container Diagram shows the main applications and services inside the PSP platform.

Each service has a single responsibility and communicates with other services through HTTP or asynchronous messaging.

---

# Containers

## API Gateway

Technology:

- ASP.NET Core
- YARP

Responsibilities:

- Reverse Proxy
- Routing
- Entry Point

---

## Payment Service

Technology:

- ASP.NET Core
- Clean Architecture

Responsibilities:

- Payment Processing
- Bank Integration
- Event Publishing

---

## Topup Service

Technology:

- ASP.NET Core
- Clean Architecture

Responsibilities:

- Recharge Processing
- MCI Integration

---

## RabbitMQ

Responsibilities:

- Event Bus
- Asynchronous Communication

---

## Mock Bank API

Simulates the external banking provider.

---

## Mock MCI API

Simulates the external mobile operator.

---

# Container View

```
                Client
                   |
                   ▼
          +----------------+
          | API Gateway    |
          +--------+-------+
                   |
                   ▼
          +----------------+
          | Payment Service|
          +--------+-------+
                   |
          +--------+--------+
          |                 |
          ▼                 ▼
     Mock Bank         RabbitMQ
                             |
                             ▼
                    +----------------+
                    | Topup Service  |
                    +--------+-------+
                             |
                             ▼
                        Mock MCI
```
