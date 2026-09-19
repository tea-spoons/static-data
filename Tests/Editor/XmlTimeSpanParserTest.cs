namespace TeaSpoons.StaticData.Editor.Tests
{
    using NUnit.Framework;

    public class XmlTimeSpanParserTest
    {
        private XmlValueParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new XmlValueParser();
        }

        [Test]
        public void TestParser()
        {
            AssertTimeSpanSeconds(0, "");

            AssertTimeSpanSeconds(4, "PT4S");
            AssertTimeSpanSeconds(4 * 60, "PT4M");
            AssertTimeSpanSeconds(4 * 60, "PT240S");
            AssertTimeSpanSeconds(4 * 60 * 60, "PT4H");
            AssertTimeSpanSeconds(4 * 60 * 60, "PT240M");
            AssertTimeSpanSeconds(4 * 60 * 60, "PT3H60M");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24, "P4D");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24, "P2DT48H");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 7, "P28D");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 7, "P27DT24H");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 30, "P4M");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 30, "P3M30D");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 30, "P3M29DT24H");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 365, "P4Y");
            AssertTimeSpanSeconds(4 * 60 * 60 * 24 * 365, "P3Y11M34DT23H59M60S");
        }

        private void AssertTimeSpanSeconds(int expectedSeconds, string s)
        {
            Assert.AreEqual(expectedSeconds, parser.ParseTimeSpan(s).TotalSeconds);
        }
    }
}
