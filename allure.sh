#!/usr/bin/env bash
# Прогон тестов + локальный Allure-отчёт в браузере.
# Аргументы передаются в dotnet test: ./allure.sh --filter "Category=API"
set -uo pipefail
cd "$(dirname "$0")"

RESULTS=bin/Debug/net8.0/allure-results
rm -rf "$RESULTS"          # иначе в отчёт попадут результаты прошлых прогонов
dotnet test "$@"           # код выхода не проверяем: отчёт нужен и при падениях
allure serve "$RESULTS"
