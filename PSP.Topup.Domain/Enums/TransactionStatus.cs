namespace PSP.Topup.Domain.Enums;

/// <summary>
/// Represents the current state of a topup transaction.
/// </summary>
public enum TransactionStatus
{
    Pending = 0,

    BankApproved = 1,

    TopupSucceeded = 2,

    ConfirmationSent = 3,

    ReversalSent = 4,

    Failed = 5
}
