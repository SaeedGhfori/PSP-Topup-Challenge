# ADR-003: External Service Simulation

## Status

Accepted

---

## Context

The real Bank and MCI APIs are external systems and are not part of this solution.

Development should not depend on external services.

---

## Decision

Dedicated Mock services are implemented.

- PSP.Mock.Bank.Api
- PSP.Mock.MCI.Api

These services simulate real provider behavior for local development and integration testing.

---

## Consequences

### Advantages

- Independent development
- Repeatable testing
- No dependency on external environments
