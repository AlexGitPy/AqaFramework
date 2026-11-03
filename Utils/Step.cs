using Allure.Net.Commons;

namespace AqaFramework.Utils;

/// <summary>
/// Шаг теста: попадает в Allure-отчёт как отдельный step (с вложенными шагами,
/// статусом и временем) и дублируется в консоль.
/// </summary>
public static class Step
{
    public static Task RunAsync(string name, Func<Task> action)
    {
        Log.Write(name);
        return AllureApi.Step(name, action);
    }

    public static Task<T> RunAsync<T>(string name, Func<Task<T>> action)
    {
        Log.Write(name);
        return AllureApi.Step(name, action);
    }

    public static void Run(string name, Action action)
    {
        Log.Write(name);
        AllureApi.Step(name, action);
    }
}
