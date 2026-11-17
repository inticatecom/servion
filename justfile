default:
   @just --list

api:
   dotnet run --project apps/api

build-api:
   dotnet build apps/api

format-api:
   dotnet format apps/api --no-restore

format-check-api:
   dotnet format apps/api --verifiy-no-changes --no-restore

migration-api name:
   dotnet ef migrations add {{name}} --project apps/api

migrate-api:
   dotnet ef database update --project apps/api

migrations-api:
   dotnet ef migrations list --project apps/api

migration-remove-api:
   dotnet ef migrations remove --project apps/api
