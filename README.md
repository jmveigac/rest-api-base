# rest-api-base

Minimal C# and ASP.NET Core 10 Web API template with a controller-based CRUD example, dependency injection, a health endpoint, Swagger UI, integration tests, and GitHub Actions CI.

## Requirements

- .NET SDK 10.0.302 or a compatible .NET 10 patch release, as configured in `global.json`.
- C# and ASP.NET Core 10.

## Use the template

```sh
dotnet restore
dotnet run --launch-profile http
```

The `http` launch profile runs at `http://localhost:5063`. The `https` profile runs at `https://localhost:7226` and also listens on `http://localhost:5063`. Both profiles use the `Development` environment.

Swagger UI is available in Development at:

```text
http://localhost:5063/swagger/index.html
```

The health endpoint is available at:

```text
http://localhost:5063/health
```

## Validate locally

Run the same quality gates used by CI:

```sh
dotnet restore rest-api-base.sln
dotnet format rest-api-base.sln --verify-no-changes --no-restore
dotnet build rest-api-base.sln --configuration Release --no-restore --warnaserror
dotnet test tests/rest-api-base.Tests/rest-api-base.Tests.csproj --configuration Release --no-restore
```

Review NuGet packages with:

```sh
dotnet package list --outdated
dotnet package list --vulnerable --include-transitive
```
