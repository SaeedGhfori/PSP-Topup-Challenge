# End-to-End Payment & Top-up Flow

## Overview

This diagram represents the complete business flow across all services.

---

```mermaid
sequenceDiagram

    actor POS

    participant Gateway
    participant Payment
    participant Bank
    participant SQL
    participant RabbitMQ
    participant Topup
    participant MCI

    POS->>Gateway: Submit Payment

    Gateway->>Payment: Forward Request

    Payment->>Payment: Validate

    Payment->>Bank: Purchase

    alt Payment Approved

        Bank-->>Payment: Success

        Payment->>SQL: Save Payment

        Payment->>RabbitMQ: Publish PaymentSucceeded

        RabbitMQ->>Topup: Consume Event

        Topup->>MCI: Recharge Request

        alt Recharge Success

            MCI-->>Topup: Success

            Topup->>SQL: Update Completed

        else Recharge Failed

            MCI-->>Topup: Failed

            Topup->>SQL: Update Failed

        end

        Payment-->>Gateway: Success

        Gateway-->>POS: HTTP 200

    else Payment Failed

        Bank-->>Payment: Failed

        Payment->>SQL: Save Failed Payment

        Payment-->>Gateway: Payment Failed

        Gateway-->>POS: HTTP 400

    end
```

---

## Business Flow

1. POS submits payment.
2. Gateway forwards the request.
3. Payment validates the request.
4. Bank authorizes or rejects the payment.
5. On success, Payment stores the transaction and publishes an event.
6. Topup consumes the event.
7. Topup calls the Mobile Operator.
8. Recharge status is updated.
9. Final state is persisted.
