# PSP.Mock.MCI.Api

## Overview

`PSP.Mock.MCI.Api` is a mock implementation of the external **MCI Top-up API**.

This service simulates the behavior of the real MCI system and is used only during **local development**, **integration testing**, and **end-to-end testing**.

The purpose of this project is to allow internal services to be developed and tested without depending on the availability of the real MCI infrastructure.

> **Important**
>
> This project is **not** an implementation of the real MCI service.
>
> It is only a simulation of the external API based on expected business scenarios.
>
> Once the official MCI API documentation is provided, the request/response contracts can be updated to match the real API without changing the business flow of the application.

---

# Business Flow

The Payment/Topup Service sends a top-up request to the external MCI service.

The request contains:

- Mobile Number
- Top-up Amount
- Request Identifier

The MCI service processes the request and returns one of several possible business outcomes.

Those outcomes may include:

- Successful top-up
- Request accepted and pending
- Failed transaction
- Invalid mobile number
- Duplicate request
- System error
- Timeout

This mock service reproduces those scenarios to help developers test different application behaviors.

---

# System Architecture

```
PSP.Payment.Api
        │
        ▼
PSP.Topup.Api
        │
        ▼
PSP.Mock.MCI.Api
        │
        ▼
(Real MCI API in Production)
```

During development:

```
Topup Service
        │
        ▼
Mock MCI
```

Production:

```
Topup Service
        │
        ▼
Real MCI API
```

The only expected change between Development and Production is the **Base URL**.

No business logic should change.

---

# Base URL

```
/api/v1/mci
```

---

# Available APIs

| Method | Endpoint | Description |
|---------|----------|-------------|
| POST | `/topup` | Creates a mock top-up request |
| GET | `/topup/{referenceNumber}` | Returns the current status of a top-up request |

---

# Topup Request

**POST**

```
/api/v1/mci/topup
```

### Request Body

```json
{
    "mobileNumber": "09121234561",
    "amount": 50000,
    "requestId": "REQ-1001"
}
```

---

# Topup Response

Example

```json
{
    "success": true,
    "status": "Success",
    "message": "Topup completed successfully.",
    "referenceNumber": "MCI202600000001"
}
```

---

# Inquiry

**GET**

```
/api/v1/mci/topup/{referenceNumber}
```

Example

```
GET /api/v1/mci/topup/MCI202600000001
```

---

# Mock Scenarios

The response is determined by the last digit of the mobile number.

| Last Digit | Business Result |
|------------|-----------------|
| **1** | Success |
| **2** | Pending |
| **3** | Failed |
| **4** | Invalid Mobile Number |
| **5** | Duplicate Request |
| **6** | Timeout |
| **7** | Internal System Error |
| Other | Success |

---

# Test Examples

## Success

```json
{
    "mobileNumber": "09121234561",
    "amount": 50000,
    "requestId": "REQ-1001"
}
```

---

## Pending

```json
{
    "mobileNumber": "09121234562",
    "amount": 50000,
    "requestId": "REQ-1002"
}
```

---

## Failed

```json
{
    "mobileNumber": "09121234563",
    "amount": 50000,
    "requestId": "REQ-1003"
}
```

---

## Invalid Mobile

```json
{
    "mobileNumber": "09121234564",
    "amount": 50000,
    "requestId": "REQ-1004"
}
```

---

## Duplicate

```json
{
    "mobileNumber": "09121234565",
    "amount": 50000,
    "requestId": "REQ-1005"
}
```

---

## Timeout

```json
{
    "mobileNumber": "09121234566",
    "amount": 50000,
    "requestId": "REQ-1006"
}
```

---

## Internal System Error

```json
{
    "mobileNumber": "09121234567",
    "amount": 50000,
    "requestId": "REQ-1007"
}
```

---

# Notes

- This project is **only a mock implementation** of the external MCI service.
- It is developed to remove dependency on the real MCI environment during development.
- No database is used.
- No data is persisted.
- Responses are generated in memory.
- The API contract may be updated when the official MCI documentation becomes available.
- Internal services should communicate with this mock exactly as they would communicate with the real MCI API.
