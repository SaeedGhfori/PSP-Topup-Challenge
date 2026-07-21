# Top-up Activity Diagram

## Overview

This activity diagram describes the recharge workflow executed by the Top-up Service.

The process begins after a successful payment event is received from RabbitMQ.

---

## Activity Diagram

```mermaid
flowchart TD

    Start([Start])

    Receive[Receive PaymentSucceeded Event]

    Validate[Validate Event]

    Valid{Event Valid?}

    Ignore[Ignore Event]

    Recharge[Call Mock MCI API]

    Result{Recharge Result}

    Completed[Update Status = Completed]

    Pending[Update Status = Pending]

    Failed[Update Status = Failed]

    Finish([End])

    Start --> Receive

    Receive --> Validate

    Validate --> Valid

    Valid -- No --> Ignore

    Ignore --> Finish

    Valid -- Yes --> Recharge

    Recharge --> Result

    Result -- Success --> Completed

    Result -- Pending --> Pending

    Result -- Failed --> Failed

    Completed --> Finish

    Pending --> Finish

    Failed --> Finish
```

---

## Business Rules

- Only successful payment events are processed.
- Invalid events are ignored.
- The Mobile Operator may return:
  - Success
  - Pending
  - Failed
- The recharge status is updated according to the operator response.
