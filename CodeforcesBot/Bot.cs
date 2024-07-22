using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using CodeforcesBot.Modules;
using System.Text;

namespace CodeforcesBot;

public sealed class Bot
{
    private readonly IServiceProvider _serviceProvider;

    public Bot()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton(static (sp) => new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged,
                DefaultRetryMode = RetryMode.RetryRatelimit,
                LogGatewayIntentWarnings = false,
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 128,
            }))
            .AddSingleton(static (sp) => new InteractionService(sp.GetRequiredService<DiscordSocketClient>(), new InteractionServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Info,
                EnableAutocompleteHandlers = false
            }))
            .BuildServiceProvider();
    }

    public async Task RunAsync(string token)
    {
        var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        var interactionService = _serviceProvider.GetRequiredService<InteractionService>();

        discordClient.Log += LogAsync;
        discordClient.Ready += ReadyAsync;
        discordClient.InteractionCreated += InteractionCreatedAsync;

        interactionService.Log += LogAsync;
        interactionService.SlashCommandExecuted += SlashCommandExecuted;

        await interactionService.AddModuleAsync<CodeforcesUserModule>(_serviceProvider);

        await discordClient.LoginAsync(TokenType.Bot, token);
        await discordClient.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private Task LogAsync(LogMessage logMessage)
    {
        DateTimeOffset now = TimeProvider.System.GetLocalNow();

        var sb = new StringBuilder();
        sb.Append($"[{now:HH:mm::ss}] {logMessage.Source} ({logMessage.Severity}) : {logMessage.Message}");

        Exception? exception = logMessage.Exception;
        while (exception is not null)
        {
            sb.AppendLine().Append(exception.Message);
            exception = exception.InnerException;
        }

        Console.WriteLine(sb.ToString());
        return Task.CompletedTask;
    }

    private async Task ReadyAsync()
    {
        var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        var interactionService = _serviceProvider.GetRequiredService<InteractionService>();

        await interactionService.RegisterCommandsGloballyAsync();

        await discordClient.SetStatusAsync(UserStatus.Online);
        await discordClient.SetGameAsync("Codeforces Problems", streamUrl: null, ActivityType.Competing);
    }

    private async Task InteractionCreatedAsync(SocketInteraction interaction)
    {
        if (interaction.Channel.GetChannelType() is ChannelType.DM or ChannelType.Group)
        {
            await interaction.RespondAsync("I don't serve on private channels!", ephemeral: true);
            return;
        }

        var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        var interactionService = _serviceProvider.GetRequiredService<InteractionService>();

        var context = new SocketInteractionContext(discordClient, interaction);
        await interactionService.ExecuteCommandAsync(context, _serviceProvider);
    }

    private static async Task SlashCommandExecuted(SlashCommandInfo command, IInteractionContext context, IResult result)
    {
        if (result.IsSuccess)
            return;

        await context.Interaction.RespondAsync($"{result.Error!.Value}: {result.ErrorReason}");
    }
}
