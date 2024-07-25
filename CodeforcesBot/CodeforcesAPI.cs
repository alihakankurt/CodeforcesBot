using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CodeforcesBot.Models;

namespace CodeforcesBot;

public static class CodeforcesAPI
{
    private static readonly HttpClient _client;

    static CodeforcesAPI()
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri("https://codeforces.com/api/")
        };
    }

    public static async ValueTask<CodeforcesUser?> GetUserInfoAsync(string handle, CancellationToken cancellationToken = default)
    {
        string endPoint = $"user.info?handles={handle}&checkHistoricHandles=false&lang=en";
        CodeforcesResult<CodeforcesUser[]>? result = await _client.GetFromJsonAsync<CodeforcesResult<CodeforcesUser[]>>(endPoint, cancellationToken);

        if (result is { Status: CodeforcesResultStatus.Ok })
        {
            return result.Data[0];
        }

        return null;
    }

    public static async ValueTask<CodeforcesRatingChange[]> GetRatingChangesAsync(string handle, CancellationToken cancellationToken = default)
    {
        string endPoint = $"user.rating?handle={handle}&lang=en";
        CodeforcesResult<CodeforcesRatingChange[]>? result = await _client.GetFromJsonAsync<CodeforcesResult<CodeforcesRatingChange[]>>(endPoint, cancellationToken);

        if (result is { Status: CodeforcesResultStatus.Ok })
        {
            return result.Data;
        }

        return Array.Empty<CodeforcesRatingChange>();
    }
}
