# ADR-002: API Gateway

## Status

Accepted

---

## Context

The system requires a single entry point for client applications.

---

## Decision

YARP (Yet Another Reverse Proxy) is used as the API Gateway.

---

## Responsibilities

- Request Routing
- HTTPS
- Logging

Business logic is intentionally excluded from the Gateway.

---

## Consequences

### Advantages

- Centralized routing
- Simpler client integration
- Easy service expansion
