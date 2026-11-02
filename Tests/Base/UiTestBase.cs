using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace AqaFramework.Tests.Base;

/// <summary>
/// Базовый класс UI-тестов. PageTest сам создаёт браузер, контекст и страницу
/// на каждый тест. Здесь добавлены настройки контекста и сохранение трейса
/// при падении (открыть: https://trace.playwright.dev).
/// </summary>
public abstract class UiTestBase : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
        Locale = "en-US"
    };

    [SetUp]
    public async Task StartTracingAsync()
    {
        await Context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    [TearDown]
    public async Task StopTracingAsync()
    {
        var context = TestContext.CurrentContext;
        var failed = context.Result.Outcome.Status == TestStatus.Failed;

        string? tracePath = null;
        if (failed)
        {
            var safeName = string.Join("_", context.Test.Name.Split(Path.GetInvalidFileNameChars()));
            tracePath = Path.Combine(context.WorkDirectory, "traces", $"{safeName}.zip");
        }

        // Path = null — трейс успешного теста просто выбрасывается
        await Context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
    }
}
