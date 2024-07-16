using IntegracaoKafka.Services;
using IntegracaoKafka.Services.Util;
using Microsoft.Extensions.Configuration;

#region CONFIGURAÇÃO

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Settings.URL_SERVIDOR_KAFKA = config["UrlServidorKafka"];
Settings.IS_DESENV = config["Ambiente"] == "2";

#endregion

Console.WriteLine("Iniciando consumo...");
Console.WriteLine();

CancellationTokenSource cts = new();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

MensagemService _mensagemService = new();
await _mensagemService.ConsumirTopicoAsync("api-integracao-teste", cts);

Console.WriteLine();
Console.WriteLine("Pessione [ENTER] para sair.");
Console.ReadLine();

