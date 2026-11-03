# AqaFramework — стартовый фреймворк автотестов на C#

Стек: .NET 8, NUnit 4, Playwright, HttpClient, FluentAssertions.
UI-тесты идут против https://www.saucedemo.com, API — против https://jsonplaceholder.typicode.com.

## Запуск

```bash
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install   # один раз: скачать браузеры
dotnet test
```

Если нет PowerShell: `dotnet tool install --global Microsoft.Playwright.CLI`, затем `playwright install`.

Только часть тестов:

```bash
dotnet test --filter "Category=API"
dotnet test --filter "Category=UI"
dotnet test --filter "FullyQualifiedName~LoginTests"
```

Локально браузер открывается с окном и паузой 500 мс между действиями (`.runsettings`),
в консоль пишутся шаги тестов. Быстрый прогон без окна (так запускает CI):

```bash
dotnet test -- Playwright.LaunchOptions.Headless=true Playwright.LaunchOptions.SlowMo=0
```

Другое окружение без правки конфига:

```bash
AQA_TestSettings__BaseUrl=https://staging.example.com dotnet test
```

## Структура

```
Config/        настройки из appsettings.json + переменные окружения
Pages/         Page Object (BasePage, LoginPage, InventoryPage)
Api/           API-клиенты и модели
Tests/Base/    UiTestBase: настройки браузера, трейс при падении
Utils/         Log.Step — пошаговый лог в консоль
Tests/Ui/      UI-тесты
Tests/Api/     API-тесты
.github/       CI в GitHub Actions
```

При падении UI-теста трейс сохраняется в `bin/Debug/net8.0/traces/`.
Открыть его можно на https://trace.playwright.dev — там видно каждый шаг, скриншоты и DOM.

## Что добавить дальше

- Allure-отчёты (пакет Allure.NUnit)
- Генерацию тестовых данных (Bogus — аналог JavaFaker)
- DI-контейнер для клиентов и страниц
- BDD через Reqnroll, если в целевых вакансиях есть Gherkin
