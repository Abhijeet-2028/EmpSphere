# Employee Management System

ASP.NET Core 2.1 MVC application using a three-layer structure with Oracle 19c and Entity Framework Core.

## Required Runtime

- .NET Core SDK 2.1.526
- ASP.NET Core runtime 2.1.30
- Oracle Database 19c

The included `global.json` pins the project to SDK `2.1.526`.

## Folder Structure

- `WWWROOT`: static CSS and web assets
- `Controller`: MVC controller classes ending with `Controller`
- `Model`: database entity and view model classes
- `View`: Razor views grouped by controller prefix
- `DAL`: EF Core `DbContext`, Oracle mappings, and schema script
- `BLL`: employee service interface and validation/business logic

## Oracle Setup

Run `DAL/OracleSchema.sql` against Oracle 19c as a user that can create objects in the `EMS_APP` schema.

The application reads the `Oracle19cConnection` connection string from configuration. The included development and production JSON files both point to local Oracle 19c service `ORCLPDB1` to avoid accidental startup failures from an unreachable sample host. In production, override it using the standard ASP.NET Core environment variable form:

```powershell
$env:ConnectionStrings__Oracle19cConnection="User Id=EMS_APP;Password=EmsApp#2026Strong;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=your-oracle19c-host)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=your-service-name)))"
```

## Run

```powershell
dotnet restore
dotnet build
dotnet run
```

Open the URL printed by `dotnet run` and navigate to `/Employee`.
