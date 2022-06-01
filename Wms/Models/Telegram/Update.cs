using System.Text.Json.Serialization;

namespace Wms.Models.Telegram;

public class Update
{
    [JsonPropertyName("update_id")]
    public int Id { get; set; }

    [JsonPropertyName("message")]
    public Message? Message { get; set; }

}

public class Message
{
    [JsonPropertyName("message_id")]
    public int Id { get; set; }

    [JsonPropertyName("date")]
    public int Date { get; set; }

    [JsonPropertyName("chat")]
    public Chat Chat { get; set; }
}

public class Chat
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

}
