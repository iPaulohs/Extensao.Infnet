# Configuração das variáveis de ambiente

Este projeto utiliza variáveis de ambiente e **.NET User Secrets** para sobrescrever as configurações do `appsettings.json`.

As credenciais apresentadas neste documento são destinadas exclusivamente ao ambiente de desenvolvimento local fornecido pelo `docker-compose.yml`.

## Desenvolvimento local — .NET User Secrets

Para executar a aplicação diretamente pelo Visual Studio, Rider ou `dotnet run`, recomenda-se utilizar o **.NET User Secrets**. Dessa forma, as configurações ficam associadas ao projeto e não precisam ser registradas como variáveis de ambiente permanentes no Windows.

Inicialize o User Secrets no projeto:

```powershell
dotnet user-secrets init
```

Em seguida, registre as configurações utilizadas pelo ambiente local:

```powershell
dotnet user-secrets init --project Geekhub.Backend.WebApi/Geekhub.Backend.WebApi.csproj
dotnet user-secrets set "DatabaseAdapterOptions:ConnectionString" "Host=localhost;Port=5432;Database=geekhub_db;Username=admin;Password=m7Tu2lwq4a7asdht93ARpa" --project Geekhub.Backend.WebApi/Geekhub.Backend.WebApi.csproj
dotnet user-secrets set "MinIoAdapterOptions:Endpoint" "localhost:9000" --project Geekhub.Backend.WebApi/Geekhub.Backend.WebApi.csproj
dotnet user-secrets set "TmdbAdapterOptions:ApiKey" "SUA_KEY_TMDB" --project Geekhub.Backend.WebApi/Geekhub.Backend.WebApi.csproj
dotnet user-secrets set "RabbitMQAdapterOptions:BootstrapServers" "amqp://admin:UWJhrL3d1MTgAMSq0zQfF7@localhost:5672" --project Geekhub.Backend.WebApi/Geekhub.Backend.WebApi.csproj
dotnet user-secrets set "RedisAdapterOptions:Configuration" "localhost:6379,password=nw5DUE14cKgPldcpdqJ5c9" --project Geekhub.Backend.WebApi/Geekhub.Backend.WebApi.csproj
```

Para visualizar as configurações cadastradas:

```powershell
dotnet user-secrets list
```

Para remover uma configuração específica:

```powershell
dotnet user-secrets remove "TmdbAdapterOptions:ApiKey"
```

Para remover todas as configurações:

```powershell
dotnet user-secrets clear
```

O User Secrets armazena os valores fora do diretório do projeto e os associa ao projeto por meio do `UserSecretsId`. Portanto, não é necessário cadastrar essas configurações nas variáveis de ambiente do usuário ou do sistema operacional.

As configurações acima são equivalentes a:

```json
{
  "DatabaseAdapterOptions": {
    "ConnectionString": "Host=localhost;Port=5432;Database=geekhub_db;Username=admin;Password=m7Tu2lwq4a7asdht93ARpa"
  },
  "MinIoAdapterOptions": {
    "Endpoint": "localhost:9000"
  },
  "TmdbAdapterOptions": {
    "ApiKey": "SUA_KEY_TMDB"
  },
  "RabbitMQAdapterOptions": {
    "BootstrapServers": "amqp://admin:UWJhrL3d1MTgAMSq0zQfF7@localhost:5672"
  },
  "RedisAdapterOptions": {
    "Configuration": "localhost:6379,password=nw5DUE14cKgPldcpdqJ5c9"
  }
}
```

## Docker

Quando a aplicação for executada em Docker, as configurações devem ser fornecidas pelo próprio ambiente de execução dos containers através do `docker-compose.yml`.

As configurações utilizadas pelo ambiente Docker são:

```yaml
environment:
  DatabaseAdapterOptions__ConnectionString: "Host=postgres;Port=5432;Database=geekhub_db;Username=admin;Password=m7Tu2lwq4a7asdht93ARpa"
  MinIoAdapterOptions__Endpoint: "minio:9000"
  TmdbAdapterOptions__ApiKey: "SUA_KEY_TMDB"
  RabbitMQAdapterOptions__BootstrapServers: "amqp://admin:UWJhrL3d1MTgAMSq0zQfF7@rabbitmq:5672"
  RedisAdapterOptions__Configuration: "redis:6379,password=nw5DUE14cKgPldcpdqJ5c9"
```

### Diferença entre execução local e Docker

Ao executar a aplicação diretamente na máquina, os serviços do Docker são acessados através de `localhost`:

```text
Aplicação
    │
    ├── PostgreSQL → localhost:5432
    ├── MinIO      → localhost:9000
    ├── RabbitMQ   → localhost:5672
    └── Redis      → localhost:6379
```

Quando a aplicação também está executando dentro do Docker Compose, os containers devem utilizar os nomes dos serviços como host:

```text
Aplicação
    │
    ├── PostgreSQL → postgres:5432
    ├── MinIO      → minio:9000
    ├── RabbitMQ   → rabbitmq:5672
    └── Redis      → redis:6379
```

Por isso, as configurações do User Secrets e do Docker possuem endpoints diferentes.

## Convenção de configuração do .NET

O .NET utiliza `:` para representar níveis hierárquicos na configuração:

```text
RabbitMQAdapterOptions:BootstrapServers
```

Em variáveis de ambiente, o mesmo caminho deve ser representado utilizando `__`:

```text
RabbitMQAdapterOptions__BootstrapServers
```

Assim, a variável:

```text
DatabaseAdapterOptions__ConnectionString
```

sobrescreve:

```json
{
  "DatabaseAdapterOptions": {
    "ConnectionString": "..."
  }
}
```

A precedência das configurações permite que os valores fornecidos pelo ambiente de execução sobrescrevam os valores definidos no `appsettings.json`.
