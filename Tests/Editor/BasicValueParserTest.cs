namespace TeaSpoons.StaticData.Editor.Tests
{
    using NUnit.Framework;

    public class BasicValueParserTest
    {
        private BasicValueParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new BasicValueParser();
        }

        [Test]
        public void TestBool()
        {
            AssertValue(true, "true");
            AssertValue(false, "false");
            AssertValue(true, "yes");
            AssertValue(false, "no");
            AssertValue(true, "on");
            AssertValue(false, "off");
            AssertValue(true, "1");
            AssertValue(false, "0");

            AssertValue(true, "True");
            AssertValue(false, "False");

            Assert.Throws<ValueParsingException>(() => parser.ParseBool("yikes"));
            Assert.Throws<MissingValueException>(() => parser.ParseBool(""));
            Assert.Throws<MissingValueException>(() => parser.ParseBool(null));
        }

        [Test]
        public void TestInt()
        {
            AssertValue(0, "0");
            AssertValue(1, "1");
            AssertValue(-1, "-1");
            AssertValue(10_000, "10_000");

            Assert.Throws<ValueParsingException>(() => parser.ParseInt("word"));
            Assert.Throws<MissingValueException>(() => parser.ParseInt(""));
            Assert.Throws<MissingValueException>(() => parser.ParseInt(null));
        }

        [Test]
        public void TestFloat()
        {
            AssertValue(0f, "0");
            AssertValue(1f, "1");
            AssertValue(1.5f, "1.5");
            AssertValue(-1f, "-1");
            AssertValue(10_000f, "10_000");
            AssertValue(0.00001f, "0.000_01");

            Assert.Throws<ValueParsingException>(() => parser.ParseFloat("word"));
            Assert.Throws<MissingValueException>(() => parser.ParseFloat(""));
            Assert.Throws<MissingValueException>(() => parser.ParseFloat(null));
        }

        [Test]
        public void TestVector2Int()
        {
            AssertValue(2, 4, "2x4");
            AssertValue(-1, 100, "-1x100");
            AssertValue(100, -1000, "100x-1000");
            AssertValue(0, 0, "0x0");
            AssertValue(10, 10, "10 x 10");
            AssertValue(10, 10, "10x 10");

            Assert.Throws<ValueParsingException>(() => parser.ParseVector2Int("word"));
            Assert.Throws<MissingValueException>(() => parser.ParseVector2Int(""));
            Assert.Throws<ValueParsingException>(() => parser.ParseVector2Int("10x10x10"));
            Assert.Throws<MissingValueException>(() => parser.ParseVector2Int(null));
        }

        [Test]
        public void TestVector2()
        {
            AssertValue(2f, 4f, "2x4");
            AssertValue(-1f, 100f, "-1x100");
            AssertValue(100f, -1000f, "100x-1000");
            AssertValue(0f, 0f, "0x0");
            AssertValue(2.5f, 4f, "2.5x4");
            AssertValue(2f, 4.1f, "2x4.1");
            AssertValue(2.5f, 4.1f, "2.5x4.1");
            AssertValue(-2.5f, 4f, "-2.5x4");
            AssertValue(2f, -4.1f, "2x-4.1");
            AssertValue(10f, 10f, "10 x 10");
            AssertValue(10f, 10f, "10x 10");

            Assert.Throws<ValueParsingException>(() => parser.ParseVector2Int("word"));
            Assert.Throws<MissingValueException>(() => parser.ParseVector2Int(""));
            Assert.Throws<ValueParsingException>(() => parser.ParseVector2Int("10x10x10"));
            Assert.Throws<MissingValueException>(() => parser.ParseVector2Int(null));
        }

#line hidden
        private void AssertValue(bool expected, string s)
        {
            Assert.AreEqual(expected, parser.ParseBool(s));
        }

        private void AssertValue(int expected, string s)
        {
            Assert.AreEqual(expected, parser.ParseInt(s));
        }

        private void AssertValue(float expected, string s)
        {
            Assert.That(parser.ParseFloat(s), Is.EqualTo(expected).Within(0.001f));
        }

        private void AssertValue(int x, int y, string s)
        {
            Assert.AreEqual(parser.ParseVector2Int(s), new UnityEngine.Vector2Int(x, y));
        }

        private void AssertValue(float x, float y, string s)
        {
            var actual = parser.ParseVector2(s);
            Assert.That(actual.x, Is.EqualTo(x).Within(0.001f));
            Assert.That(actual.y, Is.EqualTo(y).Within(0.001f));
        }
#line default
    }
}
