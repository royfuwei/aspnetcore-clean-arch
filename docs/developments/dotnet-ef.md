EFCore dotnet ef 使用方式
===

## `dotnet ef database`

### 更新Database `dotnet ef database update`
當`dotnet ef migrations add` 有異動時，可以使用`dotnet ef database update --project src/Infrastructure --startup-project src/Services/API` 去更新database。

![](../../assets/images/dotnet%20ef%20database%20update.png)
---

##  `dotnet ef migrations`

### 異動 EFCore Configurations 產生migration

當異動`src/Infrastructure/EFCore/Configurations`時，可以使用`dotnet ef migrations --project src/Infrastructure --startup-project src/Services/API --output-dir Persistence/EFCore/Migrations` 產生migration 資訊。

![](../../assets/images/dotnet%20ef%20migrations%20add.png)
