
#if TEASPOONS_STRUCTURED_DOCUMENTS && MARKDIG
namespace TeaSpoons.StaticData.Editor
{
    using TeaSpoons.StaticData;
    using TeaSpoons.StructuredDocuments;

    /// <summary>
    /// Deserializes a StaticDataObject from a markdown file.
    /// </summary>
    public abstract class MarkdownStaticDataImporter<TData> : StaticDataImporter<TData, StructuredMarkdownDocument, MarkdownValueParser>
        where TData : StaticDataObject
    {
    }
}
#endif
