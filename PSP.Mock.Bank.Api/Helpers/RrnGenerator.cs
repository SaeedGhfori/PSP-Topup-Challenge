namespace PSP.Mock.Bank.Api.Helpers;

public static class RrnGenerator
{
    public static string Generate()
    {
        return Random.Shared
            .NextInt64(100000000000, 999999999999)
            .ToString();
    }
}
