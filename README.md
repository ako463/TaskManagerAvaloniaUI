# Simple TaskManager with AvaloniaUI
Небольшое кроссплатформенное WPF-приложение для управления задачами с использованием Avalonia UI.

<img width="731" height="330" alt="image" src="https://github.com/user-attachments/assets/e4d0c94c-3354-46cf-9f63-e54ebb131761" />

# Архитектура

# Сборка и запуск приложения

## 1. Сборка проекта
В папке проекта вызвать
```bash
dotnet build
```

## 2. Применение миграций базы данных
Перед накатом миграции нужно установить инструменты для EF Core с помощью команды
```bash
dotnet tool install --global dotnet-ef
```
Затем применить миграцию базы данных
```bash
dotnet ef database update
```
## 3. Запуск приложения
Запустить приложение можно командой
```bash
dotnet run bin\Debug\net8.0\SimpleTaskManager.dll
```
Для ОС Windows также 
```bash
bin\Debug\net8.0\SimpleTaskManager.exe
```
