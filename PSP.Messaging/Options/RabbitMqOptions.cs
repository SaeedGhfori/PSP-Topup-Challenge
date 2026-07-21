namespace PSP.Messaging.Options;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string Host { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;
}
