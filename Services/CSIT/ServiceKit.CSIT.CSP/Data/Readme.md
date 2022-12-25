# ServiceKit

## Конфигурации

## Миграции

Команда для создания БД из миграций: dotnet ef database update --startup-project <путь>
e.g. dotnet ef database update --startup-project C:\Code\AlvaIT\ServiceKit\CSIT\ServiceKit.CSIT.CSP

Основной проект для миграций C:\Code\AlvaIT\ServiceKit\CSIT\ServiceKit.CSIT.CSP

### Полезные команды (для копипаста)
#### Добавить миграцию
   dotnet ef migrations add <name> --startup-project C:\Code\AlvaIT\ServiceKit\Services\CSIT\ServiceKit.CSIT.CSP

#### Обновить базу
   dotnet ef database update --startup-project C:\Code\AlvaIT\ServiceKit\Services\CSIT\ServiceKit.CSIT.CSP

#### Обновить базу на конкретную миграцию
   dotnet ef database update <name> --startup-project C:\Code\AlvaIT\ServiceKit\Services\CSIT\ServiceKit.CSIT.CSP

#### Удалить последнюю миграцию
   dotnet ef migrations remove --startup-project C:\Code\AlvaIT\ServiceKit\Services\CSIT\ServiceKit.CSIT.CSP

---

## Скрипты
### Чистка базы
    delete from Logs
    delete from Reports
    delete from RegisterItems
    delete from Registers

## Базы
#### База в ажуре
Data Source=tcp:alvait-dbserver.database.windows.net,1433;Initial Catalog=Ability.ServiceKit;User Id=alvaitadmin@alvait-dbserver;Password=P!@3P!@3