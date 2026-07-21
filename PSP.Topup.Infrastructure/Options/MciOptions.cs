namespace PSP.Topup.Infrastructure.Options
{
    public sealed class MciOptions
    {
        public const string SectionName = "Mci";

        public string BaseUrl { get; init; } = string.Empty;

        public int Timeout { get; init; }
    }
}
