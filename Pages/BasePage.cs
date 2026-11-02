using AqaFramework.Config;
using Microsoft.Playwright;

namespace AqaFramework.Pages;

public abstract class BasePage(IPage page)
{
    protected IPage Page { get; } = page;

    /// <summary>Относительный адрес страницы, например "/inventory.html".</summary>
    protected abstract string RelativeUrl { get; }

    public async Task OpenAsync() =>
        await Page.GotoAsync(Settings.Current.BaseUrl + RelativeUrl);
}
