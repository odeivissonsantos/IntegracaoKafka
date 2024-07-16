using IntegracaoKafka.Entities.DTOs;
using IntegracaoKafka.Services;

Console.WriteLine("Iniciando consumo...");

CancellationTokenSource cts = new();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

MensagemService _mensagemService = new();

while (!cts.IsCancellationRequested)
{
   MensagemDTO? mensagem =  await _mensagemService.ConsumirTopicoAsync("detran-ba", cts);
   
    if(mensagem != null)
        Console.WriteLine($"ID = {mensagem?.Id}; TÍTULO: {mensagem?.Titulo}; TEXTO = {mensagem?.Texto}");
}

Console.WriteLine("Pessione [ENTER] para sair.");
Console.ReadLine();

