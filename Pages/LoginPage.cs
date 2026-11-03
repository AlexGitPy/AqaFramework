using AqaFramework.Utils;
using Microsoft.Playwright;

namespace AqaFramework.Pages;

public class LoginPage(IPage page) : BasePage(page)
{
    protected override string RelativeUrl => "/";

    private ILocator UsernameInput => Page.Locator("#user-name");
    private ILocator PasswordInput => Page.Locator("#password");
    private ILocator LoginButton => Page.Locator("#login-button");

    public ILocator ErrorMessage => Page.Locator("[data-test='error']");

    public async Task<InventoryPage> LoginAsync(string username, string password)
    {
        Log.Step($"Логин: username='{username}'");
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
        return new InventoryPage(Page);
    }
}
