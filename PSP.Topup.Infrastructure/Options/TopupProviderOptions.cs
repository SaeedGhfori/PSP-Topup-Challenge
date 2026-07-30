namespace PSP.Topup.Infrastructure.Options;

public sealed class TopupProviderOptions
{
    public const string SectionName = "TopupProvider";

    public TopupProviderType Provider { get; init; } = TopupProviderType.Mci;

    public MciOptions Mci { get; init; } = new();

    public IrancellOptions Irancell { get; init; } = new();
}

public enum TopupProviderType
{
    Mci,
    Irancell
}

public sealed class MciOptions
{
    public string BaseUrl { get; init; } = string.Empty;

    public int Timeout { get; init; }
}

public sealed class IrancellOptions
{
    public string BaseUrl { get; init; } = string.Empty;

    public int Timeout { get; init; }
}
