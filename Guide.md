### Trabalhando com migrations

- 1.  Para adicionar uma nova migration acesse pelo terminal do .NET/Powershell a mesma pasta onde esta o arquivo csproj.



- 2.  Execute: dotnet ef migrations add <NomeDaMigration> -c <NomeDoContextoDoBancoDeDados> -o "Infra/Data/Migrations" 
	- -> Exemplo: dotnet ef add InitialCreate -c LocalStoreDbContext -o "Infra/Data/Migrations" 



- 3.  Para confirmar as alterações e aplica-las no banco de dados execute: dotnet ef database update