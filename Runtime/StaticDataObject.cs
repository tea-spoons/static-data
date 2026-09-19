
namespace TeaSpoons.StaticData
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class StaticDataObject : ScriptableObject
    {
        [field: SerializeField]
        public string Id { get; internal set; }

        [SerializeField]
        internal List<StaticDataObject> staticDataSubObjects;

        public override bool Equals(object obj)
        {
            if (obj is StaticDataObject other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"StaticDataObject[{Id}]";
        }
    }
}
