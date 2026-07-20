namespace PSP.Common;

public sealed record CorrelationContext(
    string CorrelationId,
    string? TraceId);
