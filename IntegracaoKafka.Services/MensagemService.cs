using Confluent.Kafka;
using IntegracaoKafka.Entities.DTOs;
using IntegracaoKafka.Services.Util;
using System.Text.Json;

namespace IntegracaoKafka.Services
{
    public class MensagemService
    {
        public MensagemService() { }

        public async Task<dynamic> IncluirAsync(MensagemDTO mensagem)
        {
            if (string.IsNullOrEmpty(mensagem.Titulo)) throw new Exception("Campo [TÍTULO] é obrigatório!");
            if (string.IsNullOrEmpty(mensagem.Texto)) throw new Exception("Campo [TEXTO] é obrigatório!");

            var config = new ProducerConfig
            {
                BootstrapServers = Settings.URL_SERVIDOR_KAFKA,
                MessageTimeoutMs = 10000 // 10 segundos de tempo limite para retorno da requisição;
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var resultado = await producer.ProduceAsync("api-integracao-teste",
                new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonSerializer.Serialize(mensagem)
                });

                if (resultado.Status != PersistenceStatus.Persisted)
                    throw new Exception($"Falha ao incluir mensagem: STATUS = {resultado.Status} - ERRO = {resultado.Message}");
            }

            return new { sucesso = "Mensagem incluída com sucesso!" };
        }

        public async Task ConsumirTopicoAsync(string nome_topico, CancellationTokenSource cts)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = Settings.URL_SERVIDOR_KAFKA,
                GroupId = $"api-integracao-teste-0",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            while (!cts.IsCancellationRequested)
            {
                using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                {
                    try
                    {
                        consumer.Subscribe(nome_topico);
                        var cr = consumer.Consume(cts.Token);

                        var json = cr.Message.Value;
                        MensagemDTO? mensagem = JsonSerializer.Deserialize<MensagemDTO>(json);

                        if (mensagem != null)
                            Console.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - ID = {mensagem?.Id}; TÍTULO: {mensagem?.Titulo}; TEXTO = {mensagem?.Texto}; DATA DE CRIAÇÃO UTC = {cr.Timestamp.UtcDateTime:dd/MM/yyyy HH:mm:ss}");

                        consumer.Commit(cr); // Realiza o commit em quais mensagens já foram retornadas.

                    }
                    catch (OperationCanceledException ocex)
                    {
                        consumer.Close();
                        return;
                    }
                }
            }

        }
    }
}
