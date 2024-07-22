using System;
using CodeforcesBot;

if (args.Length == 0)
{
    Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} <DISCORD_BOT_TOKEN>");
    return;
}

var bot = new Bot();
await bot.RunAsync(args[0]);
