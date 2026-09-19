
namespace TeaSpoons.StaticData
{
    using System;
    using UnityEngine;
    using System.Globalization;

    /// <summary>
    /// Base class for parsing values found in structured documents (see the StructuredDocuments package).
    /// </summary>
    public class BasicValueParser
    {
        public delegate bool TryParser<T>(string s, out T amount);

        #region Primitive Types
        public int ParseInt(string s, int? defaultValue = null)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                throw new MissingValueException();
            }

            if (TryParseInt(s, out var value))
            {
                return value;
            }

            throw new ValueParsingException(typeof(int), s);
        }

        public virtual bool TryParseInt(string s, out int value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = default;
                return false;
            }

            // Allow the literal to contain underscores as digit seperators, like 1_000_000.
            s = s.Replace("_", string.Empty);

            return int.TryParse(s, out value);
        }

        public float ParseFloat(string s, float? defaultValue = null)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                throw new MissingValueException();
            }

            if (TryParseFloat(s, out var value))
            {
                return value;
            }

            throw new ValueParsingException(typeof(float), s);
        }

        public virtual bool TryParseFloat(string s, out float value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = default;
                return false;
            }

            // Allow the literal to contain underscores as digit seperators, like 1.000_5.
            s = s.Replace("_", string.Empty);

            return float.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// Parses <paramref name="s"/> as a <see cref="bool"/>.
        /// </summary>
        /// <remarks>
        /// Possible values are "true, yes, on, 1" or "false, no, off, 0".
        /// Returns <paramref name="defaultValue"/> if none of these are found.
        /// </remarks>
        public bool ParseBool(string s, bool? defaultValue = null)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                throw new MissingValueException();
            }

            if (TryParseBool(s, out var value))
            {
                return value;
            }

            throw new ValueParsingException(typeof(bool), s);
        }

        /// <summary>
        /// Attempts to parse <paramref name="s"/> as a <see cref="bool"/>.
        /// </summary>
        /// <returns>
        /// Whether parsing was successful. The parsed bool is found in <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// Possible values are "true, yes, on, 1" or "false, no, off, 0".
        /// </remarks>
        public virtual bool TryParseBool(string s, out bool value)
        {
            s = s.Trim().ToLowerInvariant();
            if (s is "true" or "yes" or "on" or "1")
            {
                value = true;
                return true;
            }

            if (s is "false" or "no" or "off" or "0")
            {
                value = false;
                return true;
            }

            value = false;
            return false;
        }
        #endregion

        #region Advanced Types
        /// <summary>
        /// Parses <paramref name="s"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="s"/> must be in the format "12.5x34", two floats seperated by the letter <c>x</c>.
        /// </remarks>
        public Vector2 ParseVector2(string s, Vector2? defaultValue = default)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                throw new MissingValueException();
            }

            if (TryParseVector2(s, out var value))
            {
                return value;
            }

            throw new ValueParsingException(typeof(Vector2), s);
        }

        /// <summary>
        /// Attempts to parse <paramref name="s"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="s"/> must be in the format "12.5x34", two floats seperated by the letter <c>x</c>.
        /// </remarks>
        public virtual bool TryParseVector2(string s, out Vector2 value)
        {
            var parts = s.Split('x');
            if (parts.Length == 2 &&
                float.TryParse(parts[0].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(parts[1].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var y))
            {
                value = new Vector2(x, y);
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Parses <paramref name="s"/> as a <see cref="Vector2Int"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="s"/> must be in the format "12x34", two integers seperated by the letter <c>x</c>.
        /// </remarks>
        public Vector2Int ParseVector2Int(string s, Vector2Int? defaultValue = default)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                throw new MissingValueException();
            }

            var parts = s.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0].Trim(), out var x) &&
                int.TryParse(parts[1].Trim(), out var y))
            {
                return new Vector2Int(x, y);
            }

            throw new ValueParsingException(typeof(Vector2Int), s);
        }

        /// <summary>
        /// Attempts to parse <paramref name="s"/> as a <see cref="Vector2Int"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="s"/> must be in the format "12x34", two integers seperated by the letter <c>x</c>.
        /// </remarks>
        public virtual bool TryParseVector2Int(string s, out Vector2Int value)
        {
            var parts = s.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0].Trim(), out var x) &&
                int.TryParse(parts[1].Trim(), out var y))
            {
                value = new Vector2Int(x, y);
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc cref="PrettyTimeSpanParser.Parse(string, TimeSpan?)"/>
        public virtual TimeSpan ParseTimeSpan(string s, TimeSpan? defaultValue = null)
        {
            return PrettyTimeSpanParser.Parse(s, defaultValue);
        }

        /// <inheritdoc cref="PrettyTimeSpanParser.TryParse(string, out TimeSpan)/>
        public virtual bool TryParseTimeSpan(string s, out TimeSpan value)
        {
            return PrettyTimeSpanParser.TryParse(s, out value);
        }

        /// <summary>
        /// Parses <paramref name="s"/> as a value for the given enum type <typeparamref name="T"/>.
        /// </summary>
        public virtual T ParseEnum<T>(string s)
            where T : Enum
        {
            if (Enum.TryParse(typeof(T), s, true, out var result))
            {
                return (T)result;
            }

            throw new ValueParsingException(typeof(T), s);
        }

        /// <summary>
        /// Parses <paramref name="s"/> as a value for the given enum type <typeparamref name="T"/>.
        /// </summary>
        public virtual T ParseEnum<T>(string s, T defaultValue)
            where T : Enum
        {
            if (Enum.TryParse(typeof(T), s, true, out var result))
            {
                return (T)result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Attempts to parse <paramref name="s"/> as a value for the given enum type <typeparamref name="T"/>.
        /// </summary>
        public virtual bool TryParseEnum<T>(string s, out T value)
        {
            if (Enum.TryParse(typeof(T), s, true, out var genericValue))
            {
                value = (T)genericValue;
                return true;
            }

            value = default;
            return false;
        }
        #endregion

        #region References
        /// <summary>
        /// Parses <paramref name="s"/> as an amount of <typeparamref name="TReference"/>, with <typeparamref name="TReference"/> being a <see cref="StaticDataObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of thing that the amount is parsed for.</typeparam>
        /// <param name="amountParser">The parser to parse the left side of <paramref name="s"/> into the amount.</param>
        /// <param name="emptyIsZero">If <c>true</c>, an <paramref name="s"/> can be <c>null</c> or empty, and will be interpreted as a "zero of <typeparamref name="T"/>".</param>
        /// <returns>A tuple with the amount and a <see cref="StaticDataReference{T}"/>.</returns>
        /// <remarks>
        /// <paramref name="s"/> must be in the format "{amount} {reference}".
        /// </remarks>
        public (TAmount amount, StaticDataReference<TReference> type) ParseAmount<TAmount, TReference>(string s, TryParser<TAmount> amountParser, bool emptyIsZero = false)
            where TReference : StaticDataObject
        {
            if (string.IsNullOrEmpty(s))
            {
                if (emptyIsZero)
                {
                    return (default, StaticDataReference<TReference>.Null);
                }
                throw new MissingValueException();
            }

            if (TryParseAmount<TAmount, TReference>(s, amountParser, out var result))
            {
                return result;
            }

            throw new ValueParsingException($"{typeof(TAmount).Name} amount of {typeof(TReference).Name}", s);
        }

        /// <summary>
        /// Attempts to parse <paramref name="s"/> as an amount of <typeparamref name="TReference"/>, with <typeparamref name="TReference"/> being a <see cref="StaticDataObject"/>.
        /// </summary>
        /// <typeparam name="TReference">The type of thing that the amount is parsed for.</typeparam>
        /// <param name="amountParser">The parser to parse the left side of <paramref name="s"/> into the amount.</param>
        /// <remarks>
        /// <paramref name="s"/> must be in the format "{amount} {reference}".
        /// </remarks>
        public bool TryParseAmount<TAmount, TReference>(string s, TryParser<TAmount> amountParser, out (TAmount amount, StaticDataReference<TReference> reference) result)
            where TReference : StaticDataObject
        {
            var parts = s.Split(' ');
            if (parts.Length == 2 &&
                amountParser(parts[0].Trim(), out var amount) &&
                TryParseReference<TReference>(parts[1].Trim(), out var reference))
            {
                result = (amount, reference);
                return true;
            }

            result = (default, StaticDataReference<TReference>.Null);
            return false;
        }

        /// <summary>
        /// Creates a <see cref="StaticDataReference{T}"/> to reference a <see cref="StaticDataObject"/> that is identified through the data contained in <paramref name="s"/>.
        /// </summary>
        /// <param name="validateFunction">The reference key is passed to this function (if not null). If it returns <c>false</c>, this method returns <c>false</c>.</param>
        /// <exception cref="ValueParsingException">Thrown if <paramref name="s"/> cannot be parsed into a valid reference.</exception>
        public StaticDataReference<T> ParseReference<T>(string s, Predicate<string> validateFunction = null)
            where T : StaticDataObject
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new MissingValueException();
            }

            if (TryParseReference<T>(s, validateFunction, out var result))
            {
                return result;
            }
            throw new ValueParsingException("reference", s);
        }

        /// <summary>
        /// Attempts to create a <see cref="StaticDataReference{T}"/> to reference a <see cref="StaticDataObject"/> that is identified through the data contained in <paramref name="s"/>.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="s"/> could be parsed into a valid reference.</returns>
        public bool TryParseReference<T>(string s, out StaticDataReference<T> reference)
            where T : StaticDataObject
        {
            return TryParseReference(s, null, out reference);
        }

        /// <summary>
        /// Attempts to create a <see cref="StaticDataReference{T}"/> to reference a <see cref="StaticDataObject"/> that is identified through the data contained in <paramref name="s"/>.
        /// </summary>
        /// <param name="validateFunction">The reference key is passed to this function (if not null). If it returns <c>false</c>, this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="s"/> could be parsed into a valid reference.</returns>
        public virtual bool TryParseReference<T>(string s, Predicate<string> validateFunction, out StaticDataReference<T> reference)
            where T : StaticDataObject
        {
            if (string.IsNullOrEmpty(s))
            {
                reference = StaticDataReference<T>.Null;
                return false;
            }

            if (validateFunction != null && !validateFunction(s))
            {
                reference = StaticDataReference<T>.Null;
                return false;
            }

            reference = new StaticDataReference<T>(s);
            return true;
        }
        #endregion

        #region Multiline
        public virtual string[] SplitLines(string s)
        {
            return s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion
    }
}
