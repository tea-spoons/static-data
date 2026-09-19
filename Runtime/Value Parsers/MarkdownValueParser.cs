
using System;

namespace TeaSpoons.StaticData
{
    /// <summary>
    /// A basic value parser, but it considers <c>[[link]]</c> as references in the <see cref="TryParseReference{T}(string, out StaticDataReference{T})"/> method.
    /// </summary>
    public class MarkdownValueParser : BasicValueParser
    {
        public override bool TryParseReference<T>(string s, Predicate<string> validateFunction, out StaticDataReference<T> reference)
        {
            var key = GetValidKeyOrNull(s);
            
            if (string.IsNullOrEmpty(key))
            {
                reference = default;
                return false;
            }

            if (validateFunction != null && !validateFunction(key))
            {
                reference = StaticDataReference<T>.Null;
                return false;
            }

            reference = new StaticDataReference<T>(key);
            return true;
        }

        public static string GetValidKeyOrNull(string key)
        {
            if (key is not { Length: > 4 } || !key.StartsWith("[[") || !key.EndsWith("]]"))
            {
                return null;
            }
            
            return key.Substring(2, key.Length - 4);
        }

        public override string[] SplitLines(string s)
        {
            return s.Split("<br>", StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
