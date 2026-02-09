# Simple TaskManager on AvaloniaUI
Небольшое кроссплатформенное приложение для управления задачами, созданное с использованием Avalonia UI.

<img width="624" height="365" alt="image" src="https://github.com/user-attachments/assets/59babb69-e941-4200-9ff4-5832343397c9" />

# Сборка и запуск
## 1. Сборка проекта
Перейдите в корневую директорию проекта и выполните:
```bash
dotnet build
```
## 2. Настройка базы данных
Приложение поддерживает два варианта хранения данных:
- SQLite (используется по умолчанию) — для локального использования через файл БД
- PostgreSQL — для серверного развертывания

### Конфигурация базы данных
После сборки в папке ``bin/Debug/net8.0/`` создается файл ``appsettings.json``, в котором нужно настроить подключение:
```json
{
    "Database": {
        "UsePostgres": false,
        "ConnectionStrings": {
            "Postgres": "Host=localhost;Port=5432;Database=tasks;Username=postgres;Password=admin",
            "Sqlite": "Data Source=tasks.db"
        }
    }
}
```

### Параметры конфигурации:
- UsePostgres — флаг выбора СУБД:
  - true — использовать PostgreSQL
  - false — использовать SQLite
- ConnectionStrings — строки подключения для каждой СУБД:
  - Postgres — строка подключения к PostgreSQL
  - Sqlite — путь к файлу SQLite базы данных (например, "Data Source=tasks.db" для текущей директории или "Data Source=C:\\Data\\tasks.db" для указания конкретного пути)

### Установка инструментов и применение миграций
Установите инструменты EF Core:

```bash
dotnet tool install --global dotnet-ef
```
Примените миграции к базе данных:
```bash
dotnet ef database update
```

**Примечание:** 
- Перед использованием PostgreSQL убедитесь, что сервер запущен и доступен по указанному в конфигурации адресу.
- При использовании SQLite файл базы данных будет автоматически создан по пути, указанному в параметре ``Sqlite`` строки подключения.

## 3. Запуск приложения
### Вариант 1 (через .dll):
```bash
dotnet run bin\Debug\net8.0\SimpleTaskManager.dll
```
### Вариант 2 (исполняемый файл):
- Windows:
```bash
bin\Debug\net8.0\SimpleTaskManager.exe
```
- Linux/macOS:
```bash
./bin/Debug/net8.0/SimpleTaskManager
```

## 4. Управление задачами
Приложение поддерживает горячие клавиши для быстрого управления задачами:
- Добавление новой задачи — клавиша ``Enter``
- Удаление выбранной задачи — клавиша ``Delete``
