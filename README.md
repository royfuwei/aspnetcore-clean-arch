Simple Aspnet Clean Architecture
===

學習 .NET Core 開發，試著將自己理解的clean architecture 跟 DDD (Domain Driven Design) 用 .NET Core 做出簡單的開發架構。

### Technologies
- Domain Drive Design (DDD)
- Clean Architecture
- Dependency Injection (DI)
- Inversion of Control (IoC)
- Ｍediator Pattern (中介者模式)
- CQRS 模式? (不太算是，因為目前是把Query 也用Ｍediator 方式)
- Event Driven


### 功能
- Rest API

---

## Getting Started
```sh
# start Services/API service
dotnet run src/Services/API
```

### Entity Framework Core

#### Migration
`appsettings.json`
```json
    "UseInMemoryDatabase": false,
```

在`src/Services/API` project 使用Migrations

```sh
# 
dotnet ef database update --project src/Services/API

# 初次使用 或是 有異動context 跑migration
dotnet ef migrations add InitialCreate --project src/Services/API

# update database
dotnet ef database update --project src/Services/API

# drop database
dotnet ef database drop --project src/Services/API
```
> Entity Framework Core 使用`UseInMemoryDatabase` 無法使用`dotnet ef`

---

## 開發相關
### 開發文件

- [目錄及架構](./docs/developments/arch.md)
- [EFCore dotnet ef 使用方式](./docs/developments/dotnet-ef.md)

### 規劃導入的項目
- [x] 分層規劃
- [x] DDD 導入
- [x] [MediatR](https://github.com/jbogard/MediatR)
- [x] [EntityFrameworkCore](https://learn.microsoft.com/zh-tw/ef/core/)
    - [x] SqlServer
    - [x] InMemory
    - [x] Migration
- [ ] [AutoFac](https://autofac.org/)
- [ ] [AutoMapper](https://automapper.org/)
- [ ] [NUnit](https://nunit.org/)
- [ ] [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [ ] Dockerfile
- [ ] Mongodb
- [ ] RabbitMQ

---

## Reference
- [使用 DDD 與 CQRS 模式解決微服務中的商務複雜度](https://learn.microsoft.com/zh-tw/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)
- [The Clean Code Blog - The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
