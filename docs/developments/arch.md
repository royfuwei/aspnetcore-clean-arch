目錄及架構
===

開發架構上上參考很多資訊，不過主要目的是希望能夠將開發上程式能夠解耦合，更容易進行單元測試及抽換元件。

## Clean Architecture 的規則

- 分層規則
    - **Application (ApplicationService)**: 功能開發位置
    - **Domain**: 功能開發前可以先到這一層來定義介面
    - **Infrastructure**: DB Driver, Entity Framework Core, RabbitMQ ...等
    - **Services (ExtenalService)**: 對外服務 ex: Rest api, gRPC...
- 相依性規則
    > `Domain` <- `Application` <- `Infrastructure` <- `Services`
- 跨層原則

---

## Domain Driven Design - Tactical Design

**Model-driven design**

- **Entity**: 將領域內的各項實體識別出來，不一定是DB的Entity。
- **Value Object**:表達實體的狀態(State)及計算(Computation)。
- **Domain Event**: 列出與Entity 有關的事件，說明Entity 變化及狀態。
- **Aggregate**: 聚合根，將相關的實體封裝為聚合。
- **Service**: Extenal Service, ex: API。

---
## 資料夾結構

### `./` 資料夾結構

```sh
.
├── CleanArch.sln
├── docs # 說明文件
├── README.md
└── src # 程式位置
    ├── Application # ApplicationService 功能開發位置
    ├── Domain # 功能開發前可以先到這一層來定義介面
    ├── Infrastructure # DB Driver, Entity Framework Core, RabbitMQ ...等
    └── Services # ExtenalService 對外服務 ex: Rest api, gRPC...
        └── API
```

### `src` 資料夾結構

```sh
src
├── Application # ApplicationService
│   ├── Application.csproj
│   ├── Common # 共用
│   ├── ConfigureServices.cs # Application IoC, DI
│   └── Modules # 功能模組
│       └── TodoLists
│       └── WeatherForecasts
├── Domain
│   ├── AggregatesModels # DDD Aggregate root 聚合跟
│   │   ├── TodoListAggregate
│   │   └── WeatherForecastAggregate
│   ├── Domain.csproj
│   ├── DomainEvents # DDD Domain event
│   │   └── WeatherForecastCreatedEvent.cs
│   ├── DomainServices # DDD Domain service
│   ├── IntegrationEvents 
│   └── SeedWork # DDD 基本的Entity, DomainEvent, interface ...等
├── Infrastructure
│   ├── Common # 共用
│   ├── ConfigureServices.cs
│   ├── Infrastructure.csproj
│   └── Persistence # 跟儲存相關的 EFCore, Mongodb ...等
└── Services
    └── API
        ├── API.csproj
        ├── appsettings.Development.json
        ├── appsettings.json # API Service 配置檔案，ex: db connection
        ├── ConfigureServices.cs
        ├── Controllers # api controller
        ├── Infrastructure # 目前放 EFCore migrations 相關資料
        └── Program.cs
```

### `src/Application/Modules` 資料夾結構

這邊主要是將`src/Application/Modules` UseCase 做拆分。

- **EventHandlers**: 裡面分為`DomainEventHandlers`, `IntegrationEventHandlers`,用Event Driven 來解耦條件。
- **Repositories**: 定義`IRepository`，可DI 不同資料庫來源。
- **UseCases**: 用`CQRS` 分為 `Query`, `Command`，使用`ViewModels` 區分 `Entity` 資料。

```sh
src/Application/Modules
├── TodoLists
└── WeatherForecasts
    ├── EventHandlers
    │   ├── DomainEventHandlers
    │   └── IntegrationEventHandlers
    ├── Repositories
    │   ├── EFcore
    │   └── InMemory
    └── UseCases
        ├── Commands
        ├── Queries
        └── ViewModels
```