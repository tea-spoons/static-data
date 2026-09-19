
namespace TeaSpoons.StaticData
{
    using System;
    using System.Text.RegularExpressions;
    using static TimeSpanParsingConstants;

    /// <summary>
    /// A parser for xml-specific value formats.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///   <item>
    ///     <see cref="ParseTimeSpan(string, System.TimeSpan?)"/> uses the xmls <c>duration</c> format (e.g. <c>P1DT12H</c>).
    ///   </item>
    /// </list>
    /// </remarks>
    public class XmlValueParser : BasicValueParser
    {
        // TODO Support negative numbers?
        private static readonly Regex durationRegex = new Regex(@"\bP(\d+Y)?(\d+M)?(\d+D)?(T(\d+?H)?(\d+?M)?(\d+?S)?)?\b");

        public override TimeSpan ParseTimeSpan(string s, TimeSpan? defaultValue = null)
        {
            if (s == string.Empty) return TimeSpan.Zero;

            var match = durationRegex.Match(s);

            if (!match.Success) throw new ValueParsingException(typeof(TimeSpan), s);

            var years = ParseDurationLong(s, match.Groups[1], DaysPerYear * HoursPerDay * OneHour);
            var months = ParseDurationLong(s, match.Groups[2], DaysPerMonth * HoursPerDay * OneHour);
            var days = ParseDurationLong(s, match.Groups[3], HoursPerDay * OneHour);

            var hours = ParseDurationLong(s, match.Groups[5], OneHour);
            var minutes = ParseDurationLong(s, match.Groups[6], OneMinute);
            var seconds = ParseDurationLong(s, match.Groups[7], OneSecond);

            return TimeSpan.FromMilliseconds(years + months + days + hours + minutes + seconds);
        }

        private static long ParseDurationLong(string s, Group group, long factor)
        {
            if (group == null || !group.Success) return 0;

            var span = s.AsSpan(group.Index, group.Length - 1);
            return uint.Parse(span) * factor;
        }
    }
}
