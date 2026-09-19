
namespace TeaSpoons.StaticData
{
    public interface IStaticDataReference
    {
        /// <summary>
        /// The exact type of static data object that is being referenced.
        /// </summary>
        System.Type Type { get; }

        /// <summary>
        /// Returns the referenced static data object.
        /// </summary>
        /// <remarks>
        /// Used by editor code.
        /// </remarks>
        internal StaticDataObject GetBaseTypeTarget();
    }
}
