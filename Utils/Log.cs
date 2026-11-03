namespace AqaFramework.Utils;

/// <summary>
/// Пишет в консоль сразу, а не после завершения теста.
/// TestContext.Progress не буферизуется, поэтому лог виден по ходу прогона.
/// </summary>
public static class Log
{
    public static void Write(string message) =>
        TestContext.Progress.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [{TestContext.CurrentContext.Test.Name}] {message}");
}
