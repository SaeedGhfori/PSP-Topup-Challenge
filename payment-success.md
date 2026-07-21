# Payment Success Sequence

## Overview

This sequence diagram illustrates the successful payment workflow.

---

```mermaid
sequenceDiagram

    actor POS

    participant Gateway
    participant Payment
    participant Bank
    participant Database
    participant RabbitMQ

    POS->>Gateway: POST /payment

    Gateway->>Payment: Forward Request

    Payment->>Payment: Validate Request

    Payment->>Bank: Purchase Request

    Bank-->>Payment: Payment Approved

    Payment->>Database: Save Transaction (Paid)

    Payment->>RabbitMQ: Publish PaymentSucceeded

    Payment-->>Gateway: Payment Success

    Gateway-->>POS: HTTP 200 OK
```

---

## Result

- Payment Approved
- Transaction Stored
- Event Published
- Response Returned
