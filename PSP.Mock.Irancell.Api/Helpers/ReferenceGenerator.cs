namespace PSP.Mock.Irancell.Api.Helpers;

public static class ReferenceGenerator
{
    public static string Generate()
    {
        return $"IRC{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
