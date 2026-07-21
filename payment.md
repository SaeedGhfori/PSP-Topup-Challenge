# Payment Activity Diagram

## Overview

This activity diagram illustrates the business workflow of the Payment Service.

The process starts when the POS submits a payment request and ends after the payment result is returned.

---

## Activity Diagram

```mermaid
flowchart TD

    Start([Start])

    Receive[Receive Payment Request]

    Validate[Validate Request]

    Valid{Is Request Valid?}

    Reject[Return Validation Error]

    Bank[Call Bank API]

    Approved{Payment Approved?}

    SavePaid[Save Payment Transaction]

    Publish[Publish PaymentSucceeded Event]

    Success([Return Success])

    SaveFailed[Save Failed Transaction]

    Failed([Return Failed])

    End([End])

    Start --> Receive

    Receive --> Validate

    Validate --> Valid

    Valid -- No --> Reject

    Reject --> End

    Valid -- Yes --> Bank

    Bank --> Approved

    Approved -- Yes --> SavePaid

    SavePaid --> Publish

    Publish --> Success

    Success --> End

    Approved -- No --> SaveFailed

    SaveFailed --> Failed

    Failed --> End
```

---

## Business Rules

- Every request must be validated.
- Invalid requests never reach the Bank API.
- Successful payments are stored in the database.
- A successful payment publishes an integration event.
- Failed payments are persisted with a Failed status.
