# PSP.Mock.Bank.Api

## Overview

`PSP.Mock.Bank.Api` is a mock implementation of a bank acquiring service used for local development and integration testing.

The service simulates common banking operations required by the Payment Service without connecting to any real banking infrastructure.

> **Purpose**
>
> This project exists only for testing and demonstration purposes.

---

# Base URL

```
/api/v1/bank
```

---

# Available APIs

| Method | Endpoint | Description |
|---------|----------|-------------|
| POST | `/purchase` | Simulates a purchase transaction |
| POST | `/confirmation` | Simulates transaction confirmation |
| POST | `/reversal` | Simulates transaction reversal |
| POST | `/balance` | Returns a mock account balance |

---

# Purchase

Creates a mock purchase transaction.

### Request

```http
POST /api/v1/bank/purchase
```

### Body

```json
{
  "pan": "6037991234561111",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "123456"
}
```

### Success Response

```json
{
  "success": true,
  "responseCode": 0,
  "message": "Purchase successful.",
  "rrn": "123456789012"
}
```

---

# Confirmation

Confirms a successful transaction.

### Request

```http
POST /api/v1/bank/confirmation
```

### Body

```json
{
  "rrn": "123456789012"
}
```

### Response

```json
{
  "success": true,
  "responseCode": 0,
  "message": "Confirmation completed."
}
```

---

# Reversal

Reverses a previous transaction.

### Request

```http
POST /api/v1/bank/reversal
```

### Body

```json
{
  "rrn": "123456789012"
}
```

### Response

```json
{
  "success": true,
  "responseCode": 0,
  "message": "Reversal completed."
}
```

---

# Balance

Returns a mock account balance.

### Request

```http
POST /api/v1/bank/balance
```

### Body

```json
{
  "pan": "6037991234561111"
}
```

### Response

```json
{
  "success": true,
  "balance": 15000000
}
```

---

# Mock Scenarios

The response depends on the last four digits of the PAN.

| PAN Suffix | Result | Response Code |
|------------|--------|---------------|
| `9999` | Timeout | HTTP 504 |
| `8888` | Card Blocked | 54 |
| `7777` | Insufficient Funds | 51 |
| `6666` | Duplicate Transaction | 94 |
| `5555` | Internal Error | 96 |
| Any other value | Success | 0 |

---

# Response Codes

| Code | Description |
|------|-------------|
| 0 | Success |
| 14 | Invalid Card |
| 51 | Insufficient Funds |
| 54 | Card Blocked |
| 68 | Timeout |
| 94 | Duplicate Transaction |
| 96 | Internal Error |

---

# Test Examples

## Success

```json
{
  "pan": "6037991234561111",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "100001"
}
```

## Insufficient Funds

```json
{
  "pan": "6037991234567777",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "100002"
}
```

## Card Blocked

```json
{
  "pan": "6037991234568888",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "100003"
}
```

## Duplicate Transaction

```json
{
  "pan": "6037991234566666",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "100004"
}
```

## Internal Error

```json
{
  "pan": "6037991234565555",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "100005"
}
```

## Timeout

```json
{
  "pan": "6037991234569999",
  "amount": 50000,
  "terminalId": "TERM001",
  "traceNumber": "100006"
}
```

---

# Notes

- No database is used.
- No transaction state is persisted.
- All responses are generated in memory.
- RRN values are generated dynamically.
- This service is intended only for local development and integration testing.
