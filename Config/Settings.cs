using Microsoft.Extensions.Configuration;

namespace AqaFramework.Config;

/// <summary>
/// Читает appsettings.json один раз. Любое значение можно переопределить
/// переменной окружения: AQA_TestSettings__BaseUrl=https://staging.example.com
/// </summary>
public static class Settings
{
    private static readonly Lazy<TestSettings> Instance = new(Load);

    public static TestSettings Current => Instance.Value;

    private static TestSettings Load()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables(prefix: "AQA_")
            .Build();

        return configuration.GetSection("TestSettings").Get<TestSettings>()
               ?? throw new InvalidOperationException("Секция TestSettings не найдена в appsettings.json");
    }
}
