# TodoListAPI
TodoList builded in .NET Core 8.0

Para correta utilização certifique-se que seu ambiente possui o .NET Core instalado para executar comandos dotnet.

Atualize a conexão do banco com sua connectionString em TodoList/appsettings.json

Após feito isso, acesso o Console Gerenciador de Pacotes, escolha o projeto TodoList.Infra.Data e execute a geração das migrations -> Add-Migrations "nomeDaMigration".

Por ultimo execute o comando Update-Database para gerar a tabela no banco de dados.
