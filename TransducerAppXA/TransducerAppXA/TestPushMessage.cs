using System;
using Newtonsoft.Json;

namespace TransducerProtocol
{
    // TestPushMessage: mensagem enviada pelo servidor para empurrar (push) uma definição de teste ao cliente.
    // Herdará BaseMessage para que possa ser tratado por OnMessageReceived (BaseMessage).
    public class TestPushMessage : BaseMessage
    {
        public TestPushMessage()
        {
            // assegura o field type correto ao serializar
            Type = "test_push";
            // garante message id se BaseMessage define essa propriedade nula
            if (string.IsNullOrEmpty(this.MessageId))
                this.MessageId = Guid.NewGuid().ToString();
        }

        // Campos específicos do TestDefinition / TestPush
        // Ajuste nomes/atributos se seu servidor usa outras keys (use o JSON que vimos no log)
        [JsonProperty("test_id")]
        public string TestId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nominal_torque")]
        public double NominalTorque { get; set; }

        [JsonProperty("min_torque")]
        public double MinTorque { get; set; }

        [JsonProperty("max_torque")]
        public double MaxTorque { get; set; }

        [JsonProperty("repetitions")]
        public int Repetitions { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("timestamp_utc")]
        public string TimestampUtc { get; set; }
    }
}