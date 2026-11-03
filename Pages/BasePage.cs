using AqaFramework.Config;
using AqaFramework.Utils;
using Microsoft.Playwright;

namespace AqaFramework.Pages;

public abstract class BasePage(IPage page)
{
    protected IPage Page { get; } = page;

    /// <summary>Относительный адрес страницы, например "/inventory.html".</summary>
    protected abstract string RelativeUrl { get; }

    public Task OpenAsync()
    {
        var url = Settings.Current.BaseUrl + RelativeUrl;
        return Step.RunAsync($"Открыть {url}", () => Page.GotoAsync(url));
    }
}
