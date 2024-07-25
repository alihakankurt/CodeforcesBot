using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using CodeforcesBot.Models;
using System.Linq;

namespace CodeforcesBot.Modules;

public sealed class CodeforcesUserModule : InteractionModuleBase<SocketInteractionContext<SocketInteraction>>
{
    [SlashCommand("profile", "Get information about a Codeforces user")]
    public async Task ProfileCommand([Summary(nameof(handle), "The handle of the user")] string handle)
    {
        CodeforcesUser? user = await CodeforcesAPI.GetUserInfoAsync(handle);
        if (user is null)
        {
            await RespondAsync("Couldn't find a user with this handle", ephemeral: true);
            return;
        }

        Embed embed = new EmbedBuilder()
            .WithAuthor((author) =>
            {
                author.Name = user.Handle;
                author.Url = $"https://codeforces.com/profile/{user.Handle}";
                author.IconUrl = user.AvatarUrl;
            })
            .WithColor(user.Rank.GetColor())
            .WithFields(
                new EmbedFieldBuilder().WithName("Email").WithValue(user.Email ?? "Not Shared"),
                new EmbedFieldBuilder().WithName("First Name").WithValue(user.FirstName ?? "Unknown"),
                new EmbedFieldBuilder().WithName("Last Name").WithValue(user.LastName ?? "Unknown"),
                new EmbedFieldBuilder().WithName("City").WithValue(user.City ?? "Unknown"),
                new EmbedFieldBuilder().WithName("Country").WithValue(user.Country ?? "Unknown"),
                new EmbedFieldBuilder().WithName("Organization").WithValue(user.Organization ?? "Unknown"),
                new EmbedFieldBuilder().WithName("Rating").WithValue($"{user.Rating} (Max: {user.MaxRating})"),
                new EmbedFieldBuilder().WithName("Rank").WithValue($"{user.Rank} (Max: {user.MaxRank})"),
                new EmbedFieldBuilder().WithName("Contribution").WithValue(user.Contribution),
                new EmbedFieldBuilder().WithName("Registered At").WithValue(user.RegisteredAt.ToRelativeString()),
                new EmbedFieldBuilder().WithName("Last Online At").WithValue(user.LastOnlineAt.ToRelativeString())
            )
            .Build();

        await RespondAsync(embed: embed);
    }

    [SlashCommand("history", "Get the contest history of a Codeforces user")]
    public async Task RatingCommand([Summary(nameof(handle), "The handle of the user")] string handle, [Summary(nameof(count), "The maximum number of contests to show")] int count = 5)
    {
        CodeforcesRatingChange[] ratingChanges = await CodeforcesAPI.GetRatingChangesAsync(handle);
        if (ratingChanges.Length == 0)
        {
            await RespondAsync("User didn't participate in any contests", ephemeral: true);
            return;
        }

        Embed embed = new EmbedBuilder()
            .WithAuthor((author) =>
            {
                author.Name = $"{handle}'s Contest History";
                author.Url = $"https://codeforces.com/profile/{handle}";
            })
            .WithColor(Color.Blue)
            .WithFields(ratingChanges.Reverse().Take(count).Select(static (ratingChange) => new EmbedFieldBuilder()
                    .WithName($"{ratingChange.ContestName} ({ratingChange.UpdatedAt.ToRelativeString()})")
                    .WithValue($"Rank: #{ratingChange.Rank}\nRating: {ratingChange.OldRating} -> {ratingChange.NewRating} ({ratingChange.NewRating - ratingChange.OldRating:+#;-#;0})")))
            .Build();

        await RespondAsync(embed: embed);
    }
}
