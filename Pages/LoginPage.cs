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

    public Task<InventoryPage> LoginAsync(string username, string password) =>
        Step.RunAsync($"Войти как '{username}'", async () =>
        {
            await Step.RunAsync($"Ввести логин '{username}'", () => UsernameInput.FillAsync(username));
            // пароль в отчёт не пишем
            await Step.RunAsync("Ввести пароль", () => PasswordInput.FillAsync(password));
            await Step.RunAsync("Нажать Login", () => LoginButton.ClickAsync());
            return new InventoryPage(Page);
        });
}
