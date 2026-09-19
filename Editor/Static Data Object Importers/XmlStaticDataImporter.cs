#if TEASPOONS_STRUCTURED_DOCUMENTS
namespace TeaSpoons.StaticData.Editor
{
    using TeaSpoons.StaticData;
    using TeaSpoons.StructuredDocuments;

    /// <summary>
    /// Deserializes a StaticDataObject from an xml file.
    /// </summary>
    public abstract class XmlStaticDataImporter<TData> : StaticDataImporter<TData, StructuredXmlDocument, XmlValueParser>
        where TData : StaticDataObject
    {
    }
}
#endif
