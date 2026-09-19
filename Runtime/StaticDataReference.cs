
namespace TeaSpoons.StaticData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    /// <summary>
    /// A lazy reference that stores the id of a <see cref="StaticDataObject"/>, and returns the referenced object when <see cref="Get"/> is called.
    /// </summary>
    [Serializable]
    public class StaticDataReference<T> : IStaticDataReference
        where T : StaticDataObject
    {
        internal static class PropertyNames
        {
            public const string Id = nameof(id);
        }

        public static StaticDataReference<T> Null => new StaticDataReference<T>(null);

        public Type Type => typeof(T);
        [SerializeField]
        private string id;

        private T cachedReference;
#if !UNITY_EDITOR
        private bool triedLoading = false;
#endif
        private bool subscribedToCleared;

        public bool HasValue => Get() != null;


        public StaticDataReference(string id)
        {
            this.id = id;
            cachedReference = null;
        }

#if UNITY_EDITOR
        private static Dictionary<string, StaticDataObject> assetLookup;

        private static void EnsureLookupBuilt(bool forceRebuild = false)
        {
            if (!forceRebuild && assetLookup != null && assetLookup.Count > 0)
            {
                return;
            }

            assetLookup?.Clear();
            assetLookup ??= new Dictionary<string, StaticDataObject>();

            var guids = AssetDatabase.FindAssets("t:StaticDataObject");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadMainAssetAtPath(path) as StaticDataObject;
                if (asset == null) continue;

                Register(asset);
                foreach (var sub in asset.staticDataSubObjects)
                {
                    Register(sub);
                }
            }
        }

        private static void Register(StaticDataObject obj)
        {
            if (obj == null) return;
            assetLookup.TryAdd(obj.Id, obj);
        }
#endif

        /// <summary>
        /// Returns the referenced <see cref="StaticDataObject"/>.
        /// </summary>
        public T Get()
        {
#if UNITY_EDITOR
            if (cachedReference != null && cachedReference.Id == id)
            {
                return cachedReference;
            }

            if (Application.isPlaying && !string.IsNullOrEmpty(id))
            {
                CacheReference(StaticDataLibrary.Get<T>(id));
            }
            else if (!string.IsNullOrEmpty(id))
            {
                GetAndCacheReferenceFromId();
            }

            if (cachedReference != null && cachedReference.Id == id)
            {
                return cachedReference;
            }
            else
            {
                // If the above check failed, it might be because the lookup is outdated.
                GetAndCacheReferenceFromIdFallback();
                EnsureLookupBuilt(forceRebuild: true); 
            }
            
            if (cachedReference != null && cachedReference.Id == id)
            {
                return cachedReference;
            }

            if (!Application.isPlaying)
            {
                UnityEngine.Debug.LogError($"StaticDataReference<{typeof(T).Name}>.Get() failed with id '{id}'. Reference IsNull: {cachedReference == null}", null);
            }

            return null;
#else
            if (string.IsNullOrEmpty(id))
            {
                triedLoading = true;
                return null;
            }

            if (triedLoading)
            {
                return cachedReference;
            }

            try
            {
                CacheReference(StaticDataLibrary.Get<T>(id));
            }
            catch { }

            triedLoading = true;
            return cachedReference;
#endif
        }

#if UNITY_EDITOR
        private void GetAndCacheReferenceFromId()
        {
            EnsureLookupBuilt();

            if (assetLookup.TryGetValue(id, out var found) && TryCacheReference(found))
            {
                return;
            }
        }

        private void GetAndCacheReferenceFromIdFallback()
        {
            var guids = AssetDatabase.FindAssets($"t:{Type.Name} {id}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = (StaticDataObject)AssetDatabase.LoadMainAssetAtPath(path);

                if (TryCacheReference(asset))
                {
                    return;
                }

                foreach (var subAsset in asset.staticDataSubObjects)
                {
                    if (TryCacheReference(subAsset))
                    {
                        return;
                    }
                }
            }
        }

        private bool TryCacheReference(StaticDataObject staticDataObject)
        {
            if (staticDataObject != null && staticDataObject is T candidate && candidate.Id == id)
            {
                CacheReference(candidate);
                return true;
            }
            return false;
        }
#endif

        public override bool Equals(object obj)
        {
            if (obj is StaticDataReference<T> other)
            {
                return id == other.id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override string ToString()
        {
            return $"StaticDataReference[{id}]";
        }

        StaticDataObject IStaticDataReference.GetBaseTypeTarget()
        {
            return Get();
        }

        private void CacheReference(T reference)
        {
            cachedReference = reference;
#if UNITY_EDITOR

            if (cachedReference != null)
            {
                cachedReference.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }

            if (!Application.isPlaying)
            {
                return;
            }
#else
            triedLoading = true;
#endif
            if (subscribedToCleared)
            {
                return;
            }
            subscribedToCleared = true;

            StaticDataLibrary.cleared += () =>
            {
                cachedReference = null;
                subscribedToCleared = false;
#if !UNITY_EDITOR
                triedLoading = false;
#endif
            };
        }
    }
}
