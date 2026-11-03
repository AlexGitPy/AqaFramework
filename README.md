# AqaFramework — стартовый фреймворк автотестов на C#

Стек: .NET 8, NUnit 4, Playwright, HttpClient, FluentAssertions, Allure.
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

## Allure-отчёт

Один раз: `brew install allure`. Дальше:

```bash
./allure.sh                          # все тесты + отчёт в браузере
./allure.sh --filter "Category=API"  # аргументы уходят в dotnet test
```

В отчёте каждый шаг (`Utils/Step`) — отдельный step с вложенными шагами.
У HTTP-запросов вложения Request/Response с заголовками и телом,
у упавших UI-тестов — скриншот и Playwright-трейс.

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
Utils/         Step — Allure-шаги + лог в консоль
Api/LoggingHandler  полный лог HTTP-запросов и ответов
Tests/Ui/      UI-тесты
Tests/Api/     API-тесты
.github/       CI в GitHub Actions
```

При падении UI-теста трейс сохраняется в `bin/Debug/net8.0/traces/`.
Открыть его можно на https://trace.playwright.dev — там видно каждый шаг, скриншоты и DOM.

## Что добавить дальше

- Генерацию тестовых данных (Bogus — аналог JavaFaker)
- DI-контейнер для клиентов и страниц
- BDD через Reqnroll, если в целевых вакансиях есть Gherkin
