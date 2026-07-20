namespace PSP.Mock.Bank.Api.Enums;

public enum ResponseCode
{
    Success = 0,

    InsufficientFunds = 51,

    CardBlocked = 54,

    InvalidCard = 14,

    Timeout = 68,

    Duplicate = 94,

    InternalError = 96
}
