using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TransducerProtocol
{
    public class BaseMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("protocol_version")]
        public string ProtocolVersion { get; set; } = "1.0";

        [JsonProperty("message_id")]
        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("session_id")]
        public string SessionId { get; set; }
    }

    // Handshake renamed to "transducer"
    public class HelloMessage : BaseMessage
    {
        public HelloMessage() { Type = "transducer"; }
        [JsonProperty("client")]
        public string Client { get; set; }
        [JsonProperty("client_version")]
        public string ClientVersion { get; set; }
        [JsonProperty("device")]
        public string Device { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class HelloAck : BaseMessage
    {
        public HelloAck() { Type = "transducer_ack"; }
        [JsonProperty("server")]
        public string Server { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; } = "ok";
    }

    public class TracePointDto
    {
        [JsonProperty("index")]
        public int PointIndex { get; set; }
        [JsonProperty("t")]
        public double TimeMs { get; set; }
        [JsonProperty("torque")]
        public double Torque { get; set; }
        [JsonProperty("angle")]
        public double Angle { get; set; }
    }

    public class ResultMessage : BaseMessage
    {
        public ResultMessage() { Type = "result"; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("timestampUtc")]
        public string TimestampUtc { get; set; }
        [JsonProperty("torque")]
        public double Torque { get; set; }
        [JsonProperty("angle")]
        public double Angle { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("trace")]
        public List<TracePointDto> Trace { get; set; }
        [JsonProperty("frIndex")]
        public int? FrIndex { get; set; }
        [JsonProperty("frTime")]
        public double? FrTime { get; set; }
        [JsonProperty("frAngle")]
        public double? FrAngle { get; set; }
        [JsonProperty("command_id")]
        public string CommandId { get; set; }
    }

    public class ResultBatchMessage : BaseMessage
    {
        public ResultBatchMessage() { Type = "result_batch"; }
        [JsonProperty("batch_id")]
        public string BatchId { get; set; }
        [JsonProperty("results")]
        public List<ResultMessage> Results { get; set; } = new List<ResultMessage>();
    }

    public class AckMessage : BaseMessage
    {
        public AckMessage() { Type = "ack"; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("ack_message_id")]
        public string AckMessageId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; } = "received";
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
    }

    public class AckBatchMessage : BaseMessage
    {
        public AckBatchMessage() { Type = "ack_batch"; }
        [JsonProperty("batch_id")]
        public string BatchId { get; set; }
        [JsonProperty("received_count")]
        public int ReceivedCount { get; set; }
    }

    public class TestCommandMessage : BaseMessage
    {
        public TestCommandMessage() { Type = "test_command"; }
        [JsonProperty("command_id")]
        public string CommandId { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
        [JsonProperty("options")]
        public Dictionary<string, object> Options { get; set; } = new Dictionary<string, object>();
    }

    public class TestCommandAck : BaseMessage
    {
        public TestCommandAck() { Type = "test_command_ack"; }
        [JsonProperty("command_id")]
        public string CommandId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class TestProgressMessage : BaseMessage
    {
        public TestProgressMessage() { Type = "test_progress"; }
        [JsonProperty("command_id")]
        public string CommandId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("current_sample")]
        public int CurrentSample { get; set; }
        [JsonProperty("total_samples")]
        public int TotalSamples { get; set; }
        [JsonProperty("last_result_id")]
        public string LastResultId { get; set; }
    }

    public class ErrorMessage : BaseMessage
    {
        public ErrorMessage() { Type = "error"; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public static class ProtocolJson
    {
        public static string Serialize(object o) => JsonConvert.SerializeObject(o);
        public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
        public static dynamic DeserializeDynamic(string json) => JsonConvert.DeserializeObject(json);
    }
}