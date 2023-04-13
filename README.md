Simple Aspnet Clean Architecture
===

試著將自己理解的clean architecture 跟 DDD (Domain Driven Design) 用.NET Core 做出來。

1. 分層規劃
2. DDD 戰術設計
3. 

## Clean Architecture

## DDD (Domain Driven Design)


## Getting Started
```sh
# start Services/API service
dotnet run src/Services/API
```

### Entity Framework Core

#### Migration
`appsettings.json`
```json
...
    "UseInMemoryDatabase": false,
...
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


