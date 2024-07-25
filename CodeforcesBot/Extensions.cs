using System;

namespace CodeforcesBot;

public static class Extensions
{
    public static string ToRelativeString(this DateTimeOffset dateTime)
    {
        TimeSpan timeSpan = DateTimeOffset.Now - dateTime;
        return timeSpan switch
        {
            var ts when ts.TotalSeconds < 60 => "just now",
            var ts when ts.TotalMinutes < 60 => Generate(ts.Minutes, "minute"),
            var ts when ts.TotalHours < 24 => Generate(ts.Hours, "hour"),
            var ts when ts.TotalDays < 30 => Generate(ts.Days, "day"),
            var ts when ts.TotalDays < 365 => Generate(ts.Days / 30, "month"),
            _ => Generate(timeSpan.Days / 365, "year")
        };

        static string Generate(int count, string name)
        {
            return $"{count} {name}{((count > 1) ? "s" : string.Empty)} ago";
        }
    }
}
