namespace TeaSpoons.StaticData.Editor.Tests
{
    using NUnit.Framework;

    public class PrettyTimeSpanParserTest
    {
        [Test]
        public void TestKeywords()
        {
            AssertTimeSpanSeconds(0, "0");
            AssertTimeSpanSeconds(0, "instant");

            AssertTimeSpanMilliseconds(4, "4 ms");
            AssertTimeSpanMilliseconds(4, "4 millisecond");
            AssertTimeSpanMilliseconds(4, "4 milliseconds");
            AssertTimeSpanSeconds(2, "2s");
            AssertTimeSpanSeconds(4, "4 s");
            AssertTimeSpanSeconds(4, "4 second");
            AssertTimeSpanSeconds(4, "4 seconds");
            AssertTimeSpanSeconds(4 * 60, "4 m");
            AssertTimeSpanSeconds(4 * 60, "4 minute");
            AssertTimeSpanSeconds(4 * 60, "4 minutes");
            AssertTimeSpanSeconds(6 * 60 * 60, "6h");
            AssertTimeSpanSeconds(4 * 60 * 60, "4 h");
            AssertTimeSpanSeconds(4 * 60 * 60, "4 hour");
            AssertTimeSpanSeconds(4 * 60 * 60, "4hours");
            AssertTimeSpanSeconds(4 * 60 * 60, "4 hours");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24, "4 d");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24, "4 day");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24, "4 days");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 7, "4 w");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 7, "4 week");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 7, "4 weeks");
            AssertTimeSpanSeconds(3 * 60 * 60 * 24 * 30, "3month");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 30, "4 month");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 30, "4 months");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 365, "4 year");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 365, "4 years");
        }

        [Test]
        public void TestSums()
        {
            AssertTimeSpanSeconds(7 + 30 * 60, "7s, 30m");
            AssertTimeSpanSeconds(4 * 60 + 2, "4 minutes, 2seconds");
            AssertTimeSpanSeconds(2 + 60 * 60, "2second, 1 hours");
            AssertTimeSpanSeconds(2 * 24 * 60 * 60 + 6 * 60 * 60 + 4, "2 days , 6hours  ,4 seconds");
        }

        [Test]
        public void TestWeirdStrings()
        {
            Assert.Throws<ValueParsingException>(() => PrettyTimeSpanParser.Parse("bananas"));
            Assert.Throws<ValueParsingException>(() => PrettyTimeSpanParser.Parse("road 55"));
            Assert.Throws<ValueParsingException>(() => PrettyTimeSpanParser.Parse("9 day s"));
            Assert.Throws<ValueParsingException>(() => PrettyTimeSpanParser.Parse("not 3 seconds"));
            Assert.Throws<ValueParsingException>(() => PrettyTimeSpanParser.Parse("2 second, 1 banana"));
            Assert.Throws<ValueParsingException>(() => PrettyTimeSpanParser.Parse("2 seconds , 6 apples  ,3 seconds"));
        }

        private static void AssertTimeSpanMilliseconds(int expectedMilliseconds, string s)
        {
            Assert.AreEqual(expectedMilliseconds, PrettyTimeSpanParser.Parse(s).TotalMilliseconds);
        }

        private static void AssertTimeSpanSeconds(int expectedSeconds, string s)
        {
            Assert.AreEqual(expectedSeconds, PrettyTimeSpanParser.Parse(s).TotalSeconds);
        }
    }
}
