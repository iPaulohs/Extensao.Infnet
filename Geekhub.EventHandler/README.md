# Geekhub.EventHandler

Consumidor Kafka com Wolverine, usando seu proprio contrato `AccountCreatedEvent`
em `Geekhub.EventHandler.Domain`, sem referencias a projetos do Backend.
Escuta o topico `account-created` e registra o ID da conta recebida no log.

O Domain possui apenas os contratos locais, sem dependencias de mensageria.
`IncomingEventNaming` configura no consumidor o identificador de mensagem usado
pelo publicador (`Geekhub.Backend.Domain.Events.AccountCreatedEvent`), preservando
a compatibilidade dos metadados Wolverine sem compartilhar assemblies.
Os campos JSON do contrato local devem permanecer compativeis com o evento publicado.

Na raiz do repositorio:

```powershell
docker compose -f Infrastructure/docker-compose.yml up -d kafka
dotnet run --project Geekhub.EventHandler/Geekhub.EventHandler
```

Configure `KafkaAdapterOptions:BootstrapServers` e `KafkaAdapterOptions:GroupId`
no `appsettings.json`, nos user secrets ou pelas variaveis de ambiente
`KafkaAdapterOptions__BootstrapServers` e `KafkaAdapterOptions__GroupId`.
Os valores locais padrao sao `localhost:9092` e `geekhub-event-handler`.
Dentro da rede Docker da infraestrutura, use `kafka:29092` como broker.

Instancias com o mesmo grupo dividem o consumo. Um grupo sem offset salvo comeca
pelos eventos mais antigos; reinicios retomam os offsets confirmados.
O topico e provisionado automaticamente e o processamento ocorre inline pelo Wolverine.

Para verificar o fluxo, crie uma conta pela API do backend e confira o log
`Evento AccountCreated recebido para a conta ...` neste processo.
O handler atual apenas registra o recebimento, sem persistir dados.

Para novos eventos, adicione o contrato no Domain local, o handler e um listener
em `Program.cs`, usando o mesmo nome de topico definido pelo publicador.
Adicione tambem o identificador enviado pelo publicador em `IncomingEventNaming`.

## Envio de e-mails

O projeto `Geekhub.EventHandler.Infrastructure` e uma class library com uma pasta
por adapter, seguindo o Backend. A pasta `EmailAdapter` implementa `IEmailAdapter`
do Domain local usando MailKit; as opcoes e a extensao de registro ficam em
`EmailAdapter/Microsoft.Extensions.DependencyInjection`.
O adapter ja esta registrado no host e pode ser
injetado nos handlers. Criar uma conta ainda apenas registra o evento no log;
o envio deve ser chamado explicitamente pelo handler que precisar dele.

```csharp
using Geekhub.EventHandler.Domain.Adapters;
using Geekhub.EventHandler.Domain.Models;

// emailAdapter e uma instancia de IEmailAdapter recebida por injecao de dependencia.
await emailAdapter.SendAsync(
    new EmailMessage("destinatario@example.test", "Teste Geekhub", "Mensagem de teste."),
    cancellationToken);
```

`EmailMessage.IsHtml` permite enviar um corpo HTML; o padrao e texto simples.
Falhas de SMTP sao propagadas ao chamador e o envio aceita cancelamento.

Inicie o Mailpit definido na infraestrutura:

```powershell
docker compose -f Infrastructure/docker-compose.yml up -d mailpit
```

O SMTP usa `localhost:1025`, sem TLS ou autenticacao, conforme o compose.
Confira as mensagens na interface do Mailpit em http://localhost:8025.
Configure `EmailAdapterOptions` no appsettings ou use as variaveis
`EmailAdapterOptions__Host`, `EmailAdapterOptions__Port`,
`EmailAdapterOptions__FromAddress` e `EmailAdapterOptions__FromName`.
Dentro da rede Docker `platform-net`, use `mailpit` como host e mantenha a porta `1025`.
