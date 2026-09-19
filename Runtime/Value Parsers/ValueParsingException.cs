
namespace TeaSpoons.StaticData
{
    using System;

    public class ValueParsingException : Exception
    {
        public override string Message { get; }

        public ValueParsingException(Type type, string s) : this(type.Name, s)
        {
        }

        public ValueParsingException(string type, string s)
        {
            Message = $"Could not parse \"{type}\" from string \"{s}\".";
        }
    }
}
