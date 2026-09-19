
namespace TeaSpoons.StaticData
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The library that stores and loads <see cref="StaticDataObject"/>s at runtime.
    /// <see cref="StaticDataReference{T}"/>s use this class to acquire the objects they reference.
    /// </summary>
    public static class StaticDataLibrary
    {
        public delegate StaticDataObject StaticDataObjectLoadFunction(Type type, string id);

        /// <summary>
        /// The function that is called to attempt to load a <see cref="StaticDataObject"/> when it's not found in the library.
        /// </summary>
        /// <remarks>
        /// Used for a lazy loading approach. For up-front loading, use <see cref="Register(in StaticDataObject)"/> instead.
        /// </remarks>
        public static StaticDataObjectLoadFunction LoadFunction;

        internal static event Action cleared;

        private static readonly DictionaryDictionary<Type, string, StaticDataObject> library = new();

#if UNITY_EDITOR
#pragma warning disable IDE0051
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            Clear();
            LoadFunction = null;
        }
#pragma warning restore IDE0051
#endif

        /// <summary>
        /// Registers <paramref name="staticDataObject"/> in the library.
        /// </summary>
        /// <remarks>
        /// Used for up-front loading. For lazy loading, use <see cref="LoadFunction"/> instead.
        /// </remarks>
        public static void Register(in StaticDataObject staticDataObject)
        {
            // Exit early if whatever method is being used to gather the static data objects produces duplicates.
            // This can happen with sub assets when using Resources.Load, for example.
            if (library.TryGetValue(staticDataObject.GetType(), staticDataObject.Id, out var existing) &&
                existing == staticDataObject)
            {
                return;
            }

            var type = staticDataObject.GetType();
            while (type != null &&
                type != typeof(StaticDataObject))
            {
                library[type, staticDataObject.Id] = staticDataObject;
                type = type.BaseType;
            }

            if (staticDataObject.staticDataSubObjects != null)
            {
                foreach (var subObject in staticDataObject.staticDataSubObjects)
                {
                    Register(subObject);
                }
            }
        }

        /// <summary>
        /// Clears the library.
        /// </summary>
        /// <remarks>
        /// Useful for doing a runtime static data update/reload.
        /// </remarks>
        public static void Clear()
        {
            library.Clear();

            cleared?.Invoke();
            cleared = null;
        }

        /// <summary>
        /// Returns all <see cref="StaticDataObject"/>s of type <typeparamref name="T"/> in the library.
        /// </summary>
        public static IEnumerable<T> GetAll<T>()
            where T : StaticDataObject
        {
            foreach (var element in library.GetAllValues(typeof(T)))
            {
                yield return (T)element;
            }
        }

        public static T Get<T>(in string id)
            where T : StaticDataObject
        {
            if (library.TryGetValue(typeof(T), id, out var result))
            {
                return (T)result;
            }

            if (LoadFunction == null)
            {
                throw new NullReferenceException($"Could not find static data object '{id}' ({typeof(T).Name}), and no {nameof(LoadFunction)} is registered for loading static data objects.");
            }

            var typedResult = LoadFunction(typeof(T), id) as T;

            if (typedResult != null)
            {
                Register(typedResult);
                return typedResult;
            }

            throw new KeyNotFoundException($"Could not find or load static data object of type '{typeof(T).Name}' with id '{id}'.");
        }
    }
}
