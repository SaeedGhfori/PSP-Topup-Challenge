# ADR-001: Clean Architecture

## Status

Accepted

---

## Context

The project requires a maintainable and testable architecture that supports long-term development.

---

## Decision

The solution adopts Clean Architecture.

Each service is divided into the following layers:

- API
- Application
- Domain
- Infrastructure
- Persistence

---

## Consequences

### Advantages

- High maintainability
- Separation of concerns
- Testability
- Independent business logic

### Disadvantages

- More projects
- Higher initial complexity
