# PSP.Gateway.Api

## Overview

`PSP.Gateway.Api` is the single entry point for all client requests in the PSP Top-up Challenge.

The Gateway is built using **YARP (Yet Another Reverse Proxy)** and is responsible for routing incoming HTTP requests to the appropriate internal services.

This project contains **no business logic**. Its responsibility is limited to request forwarding and cross-cutting concerns.

---

# Responsibilities

- Reverse Proxy
- Request Routing
- API Entry Point
- Centralized Logging
- HTTPS Redirection

Future enhancements may include:

- Authentication
- Authorization
- Rate Limiting
- Request Correlation
- Health Checks
- Load Balancing

---

# Architecture

```
                Client / POS
                     │
                     ▼
             PSP.Gateway.Api
                     │
                     ▼
            PSP.Payment.Api
                     │
         ┌───────────┴───────────┐
         ▼                       ▼
 PSP.Mock.Bank.Api          RabbitMQ
                                  │
                                  ▼
                         PSP.Topup.Api
                                  │
                                  ▼
                         PSP.Mock.MCI.Api
```

The Gateway communicates directly with **Payment Service**.

Communication between internal services is performed independently and is **not routed through the Gateway**.

---

# Routing

Current routes:

| Route | Destination |
|--------|-------------|
| `/payment/{**catch-all}` | PSP.Payment.Api |

Example:

```
POST /payment/api/payments
```

↓

Forwarded to

```
https://localhost:{PaymentPort}/api/payments
```

---

# Reverse Proxy Configuration

Routes and destinations are configured in:

```
appsettings.json
```

using the standard YARP configuration.

---

# OpenAPI

The Gateway exposes its own OpenAPI document and Scalar UI.

Each internal service maintains its own API documentation independently.

This project does **not** aggregate OpenAPI documents from downstream services.

---

# Technology Stack

- ASP.NET Core
- YARP Reverse Proxy
- Scalar
- Serilog

---

# Notes

- No business logic should be implemented in this project.
- The Gateway only forwards requests to downstream services.
- Internal service-to-service communication does not pass through the Gateway.
- Routing configuration can be extended by adding additional routes and clusters in `appsettings.json`.

---

# Future Improvements

- JWT Authentication
- Authorization Policies
- Rate Limiting
- Correlation ID
- Distributed Tracing
- Health Checks
- Load Balancing
- OpenTelemetry
