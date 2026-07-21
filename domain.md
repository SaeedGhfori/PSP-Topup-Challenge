# Domain Class Diagram

## Overview

This class diagram illustrates the core domain model of the PSP platform.

The domain follows Domain-Driven Design (DDD) principles and separates business entities from infrastructure concerns.

---

## Class Diagram

```mermaid
classDiagram

direction LR

class AggregateRoot{
    <<abstract>>
    +Guid Id
    +AddDomainEvent()
    +ClearDomainEvents()
}

class AuditableEntity{
    <<abstract>>
    +DateTime CreatedAt
    +DateTime UpdatedAt
}

class PaymentTransaction{
    +Guid Id
    +Money Amount
    +PhoneNumber PhoneNumber
    +PaymentStatus Status
    +string TraceNumber
    +MarkAsPaid()
    +MarkAsFailed()
}

class TopupTransaction{
    +Guid Id
    +Money Amount
    +PhoneNumber PhoneNumber
    +TopupStatus Status
    +string OperatorReference
    +Complete()
    +Fail()
}

class Money{
    +decimal Value
    +string Currency
}

class PhoneNumber{
    +string Value
}

class PaymentStatus{
    <<enumeration>>
    Created
    Pending
    Paid
    Failed
    Completed
}

class TopupStatus{
    <<enumeration>>
    Received
    Pending
    Completed
    Failed
}

AggregateRoot <|-- PaymentTransaction
AggregateRoot <|-- TopupTransaction

AuditableEntity <|-- PaymentTransaction
AuditableEntity <|-- TopupTransaction

PaymentTransaction --> Money
PaymentTransaction --> PhoneNumber
PaymentTransaction --> PaymentStatus

TopupTransaction --> Money
TopupTransaction --> PhoneNumber
TopupTransaction --> TopupStatus
```

---

## Description

### PaymentTransaction

Represents a payment initiated by a POS terminal.

---

### TopupTransaction

Represents a mobile recharge transaction after a successful payment.

---

### Money

A Value Object representing monetary values.

---

### PhoneNumber

A Value Object representing a valid mobile phone number.

---

### Enumerations

PaymentStatus and TopupStatus define the lifecycle of transactions.

---

## Design Principles

- Domain Driven Design
- Rich Domain Model
- Aggregate Root
- Value Objects
- Strongly Typed Domain
