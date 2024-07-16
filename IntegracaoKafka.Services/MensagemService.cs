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
            var config = new ProducerConfig
            {
                BootstrapServers = Settings.URL_SERVIDOR_KAFKA,
                MessageTimeoutMs = 10000 // 10 segundos de tempo limite para retorno da requisição;
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var resultado = await producer.ProduceAsync("detran-ba",
                new Message<string, string>
                {
                    Key = new Guid().ToString(),
                    Value = JsonSerializer.Serialize(mensagem)
                });

                if (resultado.Status != PersistenceStatus.Persisted)
                    throw new Exception($"Falha ao incluir mensagem: STATUS = {resultado.Status} - ERRO = {resultado.Message}");
            }

            return new { sucesso = "Mensagem incluída com sucesso!" };
        }

        public async Task<MensagemDTO?> ConsumirTopicoAsync(string nome_topico, CancellationTokenSource cts)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = "10.21.246.79:9092",
                    GroupId = $"detran-ba-group-0",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                {
                    consumer.Subscribe(nome_topico);
                    var cr = consumer.Consume(cts.Token);

                    var json = cr.Message.Value;
                    MensagemDTO? mensagem = JsonSerializer.Deserialize<MensagemDTO>(json);
                    return mensagem;
                }
            }
            catch (OperationCanceledException ocex)
            {
                return null;
            }
        }
    }
}
