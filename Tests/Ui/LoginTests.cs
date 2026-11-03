using System.Text.RegularExpressions;
using Allure.Net.Commons;
using Allure.Net.Commons.Attributes;
using AqaFramework.Config;
using AqaFramework.Pages;
using AqaFramework.Tests.Base;
using AqaFramework.Utils;

namespace AqaFramework.Tests.Ui;

[TestFixture]
[Category("UI")]
[AllureFeature("Логин")]
[Parallelizable(ParallelScope.Self)]
public class LoginTests : UiTestBase
{
    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    public async Task Login_WithValidCredentials_OpensInventory()
    {
        var loginPage = new LoginPage(Page);
        await loginPage.OpenAsync();

        var inventory = await loginPage.LoginAsync(Settings.Current.Username, Settings.Current.Password);

        await Step.RunAsync("Проверить, что открылся каталог с 6 товарами", async () =>
        {
            await Expect(Page).ToHaveURLAsync(new Regex("inventory.html"));
            await Expect(inventory.Title).ToHaveTextAsync("Products");
            await Expect(inventory.Items).ToHaveCountAsync(6);
        });
    }

    [TestCase("locked_out_user", "secret_sauce", "Sorry, this user has been locked out")]
    [TestCase("standard_user", "wrong_password", "Username and password do not match")]
    [TestCase("", "", "Username is required")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task Login_WithInvalidData_ShowsError(string username, string password, string expectedError)
    {
        var loginPage = new LoginPage(Page);
        await loginPage.OpenAsync();

        await loginPage.LoginAsync(username, password);

        await Step.RunAsync($"Проверить ошибку: '{expectedError}'", () =>
            Expect(loginPage.ErrorMessage).ToContainTextAsync(expectedError));
    }
}
