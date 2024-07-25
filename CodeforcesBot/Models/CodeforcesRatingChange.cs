using System;
using System.Text.Json.Serialization;
using CodeforcesBot.JsonConverters;

namespace CodeforcesBot.Models;

public sealed class CodeforcesRatingChange
{
    [JsonPropertyName("contestId")]
    public int ContestId { get; set; }

    [JsonPropertyName("contestName")]
    public string ContestName { get; set; } = string.Empty;

    [JsonPropertyName("handle")]
    public string Handle { get; set; } = string.Empty;

    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("ratingUpdateTimeSeconds"), JsonConverter(typeof(UnixTimeJsonConverter))]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("oldRating")]
    public int OldRating { get; set; }

    [JsonPropertyName("newRating")]
    public int NewRating { get; set; }
}
