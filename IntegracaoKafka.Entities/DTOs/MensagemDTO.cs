using System.Text.Json.Serialization;

namespace IntegracaoKafka.Entities.DTOs
{
    public class MensagemDTO
    {
        public long? Id { get; set; } = new Random().NextInt64(10000000000, 99999999999);
        public string Titulo { get; set; }
        public string Texto { get; set; }
    }
}
