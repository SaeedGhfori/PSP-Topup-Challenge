using System;
using System.Collections.Generic;
using System.Text;

namespace PSP.Payment.Application.Contracts.Bank
{
    public sealed record BankConfirmationRequest(
        string Rrn);
}
