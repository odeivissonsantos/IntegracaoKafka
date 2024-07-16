using Confluent.Kafka;
using IntegracaoKafka.Entities.DTOs;
using IntegracaoKafka.Services.Util;
using System.Text.Json;

namespace IntegracaoKafka.Services
{
    public class MensagemService
    {
        public MensagemService() { }

        public async Task<dynamic> IncluirAsync(RequestMensagemDTO mensagem)
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

        public async Task<dynamic> EnviarAsync()
        {
            return new { sucesso = "Mensagem incluída com sucesso!" };
        }
    }
}
