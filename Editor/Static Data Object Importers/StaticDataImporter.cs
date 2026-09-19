
namespace TeaSpoons.StaticData.Editor
{
    using TeaSpoons.StructuredDocuments;
    using UnityEngine;
    using UnityEditor.AssetImporters;
    using System.IO;
    using System.Collections.Generic;

    public abstract class StaticDataImporter<TData, TDocument, TValueParser> : ScriptedImporter
        where TData : StaticDataObject
        where TDocument : StructuredDocument, new()
        where TValueParser : BasicValueParser, new()
    {
        protected readonly TValueParser parser = new();

        protected AssetImportContext ctx;
        private List<StaticDataObject> staticDataSubAssets;

        public override sealed void OnImportAsset(AssetImportContext ctx)
        {
            this.ctx = ctx;

            var assetPath = ctx.assetPath;
            var content = File.ReadAllText(assetPath);
            var document = new TDocument();
            document.Load(content);

            var id = Path.GetFileNameWithoutExtension(assetPath);
            var importedObject = CreateAndDeserializeInstance(document, id);

            importedObject.Id = id;

            importedObject.staticDataSubObjects = staticDataSubAssets;
            
            ctx.AddObjectToAsset("Main Asset", importedObject);
            ctx.SetMainObject(importedObject);

            // Add the result to the static data library so it's immediately referencable by
            // objects that are imported after.
            StaticDataLibrary.Register(importedObject);
        }
        
        protected abstract TData CreateAndDeserializeInstance(TDocument document, string id);

        protected TData CreateMainAsset()
        {
            return CreateMainAsset<TData>();
        }

        protected T CreateMainAsset<T>()
            where T : TData
        {
            return ScriptableObject.CreateInstance<T>();
        }

        protected T AddStaticDataSubAsset<T>(string id)
            where T : StaticDataObject
        {
            var instance = AddSubAsset<T>(id);
            instance.Id = id;
            staticDataSubAssets ??= new();
            staticDataSubAssets.Add(instance);
            return instance;
        }
        
        protected StaticDataObject AddStaticDataSubAsset(System.Type type, string id)
        {
            var instance = (StaticDataObject)AddSubAsset(type, id);
            instance.Id = id;
            staticDataSubAssets ??= new();
            staticDataSubAssets.Add(instance);
            return instance;
        }

        protected T AddSubAsset<T>(string name)
            where T : ScriptableObject
        {
            return (T) AddSubAsset(typeof(T), name);
        }

        protected ScriptableObject AddSubAsset(System.Type type, string name)
        {
            var result = ScriptableObject.CreateInstance(type);
            result.name = name;
            ctx.AddObjectToAsset(name, result);

            return result;
        }
    }
}

