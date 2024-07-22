using System;
using System.Text.Json.Serialization;
using CodeforcesBot.JsonConverters;

namespace CodeforcesBot.Models;

public sealed class CodeforcesUser
{
    [JsonPropertyName("handle")]
    public required string Handle { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }

    [JsonPropertyName("contribution")]
    public int Contribution { get; set; }

    [JsonPropertyName("rank"), JsonConverter(typeof(JsonStringEnumConverter<CodeforcesRank>))]
    public CodeforcesRank Rank { get; set; }

    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    [JsonPropertyName("maxRank"), JsonConverter(typeof(JsonStringEnumConverter<CodeforcesRank>))]
    public CodeforcesRank MaxRank { get; set; }

    [JsonPropertyName("maxRating")]
    public int MaxRating { get; set; }

    [JsonPropertyName("lastOnlineTimeSeconds"), JsonConverter(typeof(UnixTimeJsonConverter))]
    public DateTimeOffset LastOnlineAt { get; set; }

    [JsonPropertyName("registrationTimeSeconds"), JsonConverter(typeof(UnixTimeJsonConverter))]
    public DateTimeOffset RegisteredAt { get; set; }

    [JsonPropertyName("friendOfCount")]
    public int FriendCount { get; set; }

    [JsonPropertyName("avatar")]
    public string AvatarUrl { get; set; } = string.Empty;

    [JsonPropertyName("titlePhoto")]
    public string TitlePhoto { get; set; } = string.Empty;
}
