namespace BuildingBlocks.Utilities;

public static class SettingsUtils
{
    public static string GetEnvironmentValueOrDefault(string envKey, string defaultValue)
        => Environment.GetEnvironmentVariable(envKey) ?? defaultValue;

    public static int GetEnvironmentValueOrDefault(string envKey, int defaultValue)
        => int.TryParse(Environment.GetEnvironmentVariable(envKey), out int value) ? value : defaultValue;
}
