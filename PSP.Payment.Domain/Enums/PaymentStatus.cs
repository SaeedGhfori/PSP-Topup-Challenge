namespace PSP.Payment.Domain.Enums;

public enum PaymentStatus
{
    Pending = 0,

    Purchased = 1,

    Confirmed = 2,

    Reversed = 3,

    Failed = 4
}
