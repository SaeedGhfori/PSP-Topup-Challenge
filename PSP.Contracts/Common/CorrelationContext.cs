namespace PSP.Contracts.Common;

public sealed record CorrelationContext(
    string CorrelationId,
    string? TraceId);
