#if !TEASPOONS_COLLECTIONS
namespace TeaSpoons.StaticData
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A dictionary of dictionaries, with just what <see cref="StaticDataLibrary"/> needs.
    /// Stand-in for the class of the same name in the collections package (<c>System.Collections.Generic</c>),
    /// which the library uses instead when that package is in the project.
    /// </summary>
    internal sealed class DictionaryDictionary<TFirstKey, TSecondKey, TValue>
    {
        private readonly Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> dictionary = new();

        public TValue this[TFirstKey firstKey, TSecondKey secondKey]
        {
            set
            {
                if (!dictionary.TryGetValue(firstKey, out var inner))
                {
                    inner = new Dictionary<TSecondKey, TValue>();
                    dictionary.Add(firstKey, inner);
                }

                inner[secondKey] = value;
            }
        }

        public bool TryGetValue(TFirstKey firstKey, TSecondKey secondKey, out TValue value)
        {
            if (dictionary.TryGetValue(firstKey, out var inner))
            {
                return inner.TryGetValue(secondKey, out value);
            }

            value = default;
            return false;
        }

        public IEnumerable<TValue> GetAllValues(TFirstKey firstKey)
        {
            return dictionary.TryGetValue(firstKey, out var inner) ? inner.Values : Enumerable.Empty<TValue>();
        }

        public void Clear() => dictionary.Clear();
    }
}
#endif
