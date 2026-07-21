# C4 Model - Context Diagram

## Overview

The Context Diagram represents the system as a single unit and illustrates how it interacts with external actors and systems.

---

# Primary Actor

- POS (Point of Sale)

The POS sends payment requests to the PSP platform.

---

# External Systems

## Bank API

An external banking system responsible for authorizing payment transactions.

During development, this dependency is simulated by `PSP.Mock.Bank.Api`.

---

## Mobile Operator (MCI)

An external mobile operator responsible for processing recharge requests.

During development, this dependency is simulated by `PSP.Mock.MCI.Api`.

---

# PSP Top-up System

The PSP platform is responsible for:

- Receiving payment requests
- Validating payments
- Processing bank transactions
- Publishing payment events
- Processing mobile recharge requests
- Returning final transaction status

---

# Context View

```
+--------------------+
|        POS         |
+---------+----------+
          |
          |
          ▼
+-----------------------------+
|      PSP Top-up System      |
+-------------+---------------+
              |
      +-------+-------+
      |               |
      ▼               ▼
+-----------+   +-------------+
| Bank API  |   |   MCI API   |
+-----------+   +-------------+
```
