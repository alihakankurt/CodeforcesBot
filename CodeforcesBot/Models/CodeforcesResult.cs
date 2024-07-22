using System.Text.Json.Serialization;

namespace CodeforcesBot.Models;

public sealed class CodeforcesResult<TData> where TData : notnull
{
    [JsonPropertyName("status"), JsonConverter(typeof(JsonStringEnumConverter<CodeforcesResultStatus>))]
    public CodeforcesResultStatus Status { get; set; }

    [JsonPropertyName("result")]
    public TData Data { get; set; } = default!;
}
