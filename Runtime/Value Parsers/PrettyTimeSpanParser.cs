
namespace TeaSpoons.StaticData
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using static TimeSpanParsingConstants;

    /// <summary>
    /// Helps parsing strings to <see cref="TimeSpan"/>s.
    /// </summary>
    public static class PrettyTimeSpanParser
    {
        private static readonly Regex regex = new(@"^\s*(\d+)\s*([a-z]+)\s*$");
        
        private static readonly Dictionary<string, long> timeSpanKeywords = new()
        {
            { "ms", 1 },
            { "millisecond", 1 },
            { "milliseconds", 1 },
            { "s", 1000 },
            { "second", 1000 },
            { "seconds", 1000 },
            { "m", OneMinute },
            { "minute", OneMinute },
            { "minutes", OneMinute },
            { "h", OneHour },
            { "hour", OneHour },
            { "hours", OneHour },
            { "d", HoursPerDay * OneHour },
            { "day", HoursPerDay * OneHour },
            { "days", HoursPerDay * OneHour },
            { "w", DaysPerWeek * HoursPerDay * OneHour },
            { "week", DaysPerWeek * HoursPerDay * OneHour },
            { "weeks", DaysPerWeek * HoursPerDay * OneHour },
            { "month", DaysPerMonth * HoursPerDay * OneHour },
            { "months", DaysPerMonth * HoursPerDay * OneHour },
            { "year", DaysPerYear * HoursPerDay * OneHour },
            { "years", DaysPerYear * HoursPerDay * OneHour }
        };

        /// <summary>
        /// Parses a <see cref="TimeSpan"/> from adding up comma-seperated values, each consisting of an integer and a keyword.
        /// </summary>
        /// <param name="s">
        ///   Example values:
        ///   <list type="bullet">
        ///     <item>0</item>
        ///     <item>instant</item>
        ///     <item>100 ms</item>
        ///     <item>30 seconds</item>
        ///     <item>1 hour</item>
        ///     <item>1 day, 12 hours</item>
        ///   </list>
        /// </param>
        /// <param name="defaultValue">
        ///   Value to be returned if <paramref name="s"/> is <c>null</c> or empty.
        /// </param>
        /// <exception cref="ValueParsingException">Thrown if <paramref name="s"/> is not in a valid format.</exception>
        public static TimeSpan Parse(string s, TimeSpan? defaultValue = default)
        {
            if (s == null)
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                throw new MissingValueException();
            }

            if (TryParse(s, out var value))
            {
                return value;
            }

            throw new ValueParsingException(typeof(TimeSpan), s);
        }

        /// <summary>
        /// Attempts to parse a <see cref="TimeSpan"/> from adding up comma-seperated values, each consisting of an integer and a keyword.
        /// </summary>
        /// <param name="s">
        ///   Example values:
        ///   <list type="bullet">
        ///     <item>0</item>
        ///     <item>instant</item>
        ///     <item>100ms</item>
        ///     <item>30 seconds</item>
        ///     <item>1 hour</item>
        ///     <item>1 day,12 hours</item>
        ///   </list>
        /// </param>
        /// <returns><c>true</c> if <paramref name="s"/> was in a parseable format, and parsing succeeded.</returns>
        public static bool TryParse(string s, out TimeSpan value)
        {
            value = TimeSpan.Zero;
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            s = s.Trim().ToLowerInvariant();

            if (s is "0" or "instant")
            {
                return true;
            }

            var milliSeconds = 0L;
            var summands = s.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var summand in summands)
            {
                var match = regex.Match(summand);
                if (!match.Success)
                {
                    return false;
                }

                if (!int.TryParse(match.Groups[1].Value, out var amount) ||
                    !timeSpanKeywords.TryGetValue(match.Groups[2].Value, out var factor))
                {
                    return false;
                }
                
                milliSeconds += amount * factor;
            }

            value = TimeSpan.FromMilliseconds(milliSeconds);
            return true;
        }
    }
}
