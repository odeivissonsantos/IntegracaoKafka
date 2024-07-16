using Confluent.Kafka;
using IntegracaoKafka.Entities.DTOs;
using System.Text.Json;

namespace IntegracaoKafka.Services
{
    public class MensagemService
    {
        public MensagemService() { }

        public dynamic Incluir(RequestMensagemDTO mensagem)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "10.21.246.79:9092"
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                string mensagemJSON = JsonSerializer.Serialize(mensagem);

                producer.Produce("detran-ba",
                new Message<string, string>
                {
                    Key = new Guid().ToString(),
                    Value = mensagemJSON
                });

            }

            return new { sucesso = "Mensagem incluída com sucesso!" };
        }

        public dynamic Enviar()
        {
            return null;
        }
    }
}
