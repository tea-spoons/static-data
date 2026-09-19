
#if NEWTONSOFT_JSON
namespace TeaSpoons.StaticData.Editor
{
    using TeaSpoons.StaticData;
    using TeaSpoons.StructuredDocuments;

    /// <summary>
    /// Deserializes a StaticDataObject from a json file.
    /// </summary>
    public abstract class JsonStaticDataImporter<TData> : StaticDataImporter<TData, StructuredJsonDocument, BasicValueParser>
        where TData : StaticDataObject
    {
    }
}
#endif
