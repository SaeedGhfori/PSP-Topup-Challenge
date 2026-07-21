# Payment Use Case

## Overview

This diagram illustrates the responsibilities of the **Payment Service** within the PSP platform.

The service is responsible for receiving payment requests, validating them, communicating with the Bank API, and publishing successful payment events.

---

## Actors

- POS Terminal
- Bank API
- RabbitMQ

---

## Use Case Diagram

```mermaid
flowchart LR

    %% Actors
    POS["🖥️ POS Terminal"]
    BANK["🏦 Mock Bank API"]
    MQ["📨 RabbitMQ"]

    %% System
    subgraph Payment_Service["💳 PSP.Payment.Api"]

        UC1(("Submit Payment"))
        UC2(("Validate Request"))
        UC3(("Authorize Payment"))
        UC4(("Publish Payment Event"))
        UC5(("Get Payment Status"))

    end

    POS --> UC1
    POS --> UC5

    UC1 --> UC2
    UC2 --> UC3
    UC3 --> UC4

    BANK --> UC3

    UC4 --> MQ
```

---

## Description

### Submit Payment

Receives a payment request from the POS terminal.

---

### Validate Request

Validates:

- Amount
- Phone Number
- Merchant Information
- Request Integrity

---

### Authorize Payment

Calls the external Bank API to authorize the payment.

---

### Publish Payment Event

Publishes a successful payment event to RabbitMQ for downstream processing.

---

### Get Payment Status

Returns the current payment status.
