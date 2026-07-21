# Payment Use Case

## Overview

This diagram describes the primary payment capabilities exposed by the PSP platform.

The Payment Service is responsible for validating requests, communicating with the Bank API and publishing payment events.

---

```mermaid
flowchart LR

    POS["🖥️ POS Terminal"]

    UC1(("Submit Payment"))
    UC2(("Validate Payment"))
    UC3(("Authorize Bank Transaction"))
    UC4(("Publish Payment Event"))
    UC5(("Check Payment Status"))

    BANK["🏦 Bank API"]

    POS --- UC1
    POS --- UC5

    UC1 --> UC2
    UC2 --> UC3
    UC3 --> UC4

    BANK --- UC3
```

---

## Description

The payment workflow begins when the POS submits a payment request.

The Payment Service validates the request and forwards it to the external Bank API.

If the transaction is approved, a payment event is published for downstream services.
