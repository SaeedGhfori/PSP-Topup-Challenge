namespace PSP.Mock.MCI.Api.Helpers;

public static class ReferenceGenerator
{
    public static string Generate()
    {
        return $"MCI{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
