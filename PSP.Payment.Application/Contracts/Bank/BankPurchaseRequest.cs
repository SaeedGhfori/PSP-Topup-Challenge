using System;
using System.Collections.Generic;
using System.Text;

namespace PSP.Payment.Application.Contracts.Bank
{
    public sealed record BankPurchaseRequest(
        string Pan,
        decimal Amount,
        string TerminalId,
        string TraceNumber);
}
