using System;
using System.Collections.Generic;
using System.Text;

namespace PSP.Payment.Application.Contracts.Bank
{
    public sealed record BankPurchaseResponse(
        bool Success,
        string Rrn,
        int ResponseCode,
        string Message);
}
