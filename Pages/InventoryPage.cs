using Microsoft.Playwright;

namespace AqaFramework.Pages;

public class InventoryPage(IPage page) : BasePage(page)
{
    protected override string RelativeUrl => "/inventory.html";

    public ILocator Title => Page.Locator(".title");
    public ILocator Items => Page.Locator(".inventory_item");
}
