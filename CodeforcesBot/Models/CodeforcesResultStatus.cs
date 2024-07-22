using System.Text.Json.Serialization;

namespace CodeforcesBot.Models;

public enum CodeforcesResultStatus
{
    [JsonPropertyName("FAILED")]
    Failed,

    [JsonPropertyName("OK")]
    Ok,
}
