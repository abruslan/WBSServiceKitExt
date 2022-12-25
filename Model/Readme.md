# ServiceKit

## Конфигурации

## Миграции

Команда для создания БД из миграций: dotnet ef database update --startup-project <путь к ServiceKit.Model.WBS >
e.g. dotnet ef database update --startup-project C:\Code\AlvaIT\ServiceKit\Model\ServiceKit.Model 

Основной проект для миграций C:\Code\AlvaIT\ServiceKit\Model\ServiceKit.Model.WBS

### Полезные команды (для копипаста)
#### Добавить миграцию
   dotnet ef migrations add <name> --startup-project C:\Code\AlvaIT\ServiceKit\Model\ServiceKit.Model.WBS

#### Обновить базу
   dotnet ef database update --startup-project C:\Code\AlvaIT\ServiceKit\Model\ServiceKit.Model.WBS

#### Обновить базу на конкретную миграцию
   dotnet ef database update <name> --startup-project C:\Code\AlvaIT\ServiceKit\Model\ServiceKit.Model.WBS

#### Удалить последнюю миграцию
   dotnet ef migrations remove --startup-project C:\Code\AlvaIT\ServiceKit\Model\ServiceKit.Model.WBS


