using System.Text.Json.Serialization;
using Discord;

namespace CodeforcesBot.Models;

public enum CodeforcesRank
{
    [JsonPropertyName("newbie")]
    Newbie,

    [JsonPropertyName("pupil")]
    Pupil,

    [JsonPropertyName("specialist")]
    Specialist,

    [JsonPropertyName("expert")]
    Expert,

    [JsonPropertyName("candidate master")]
    CandidateMaster,

    [JsonPropertyName("master")]
    Master,

    [JsonPropertyName("international master")]
    InternationalMaster,

    [JsonPropertyName("grandmaster")]
    Grandmaster,

    [JsonPropertyName("international grandmaster")]
    InternationalGrandmaster,

    [JsonPropertyName("legendary grandmaster")]
    LegendaryGrandmaster,
}

public static class CodeforcesRankExtensions
{
    public static Color GetColor(this CodeforcesRank rank)
    {
        return rank switch
        {
            CodeforcesRank.Newbie => Color.DarkGrey,
            CodeforcesRank.Pupil => Color.Green,
            CodeforcesRank.Specialist => Color.Teal,
            CodeforcesRank.Expert => Color.Blue,
            CodeforcesRank.CandidateMaster => Color.Purple,
            CodeforcesRank.Master or CodeforcesRank.InternationalMaster => Color.Orange,
            CodeforcesRank.Grandmaster or CodeforcesRank.InternationalGrandmaster or CodeforcesRank.LegendaryGrandmaster => Color.Red,
            _ => Color.Default
        };
    }
}
