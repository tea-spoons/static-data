# Static Data
Management for gamedesign static data. Prepares and loads files, stores them in a runtime library and allows cross-referencing.

## Usage
Create a `StaticDataObject` class for a type of static data object:
```csharp
public class BuildingData : StaticDataObject
{
    [field: SerializeField]
    public string Title { get; internal set; }
    
    [field: SerializeField]
    public string Description { get; internal set; }
    
    [field: SerializeField]
    public int Cost { get; internal set; }
    
    [SerializeField]
    internal StaticDataReference<ResourceData> producedGood;
    public ResourceData ProducedGood => producedGood.Get();
    
    [field: SerializeField]
    public Building Prefab { get; internal set; }
}
```

For this example, we're using a markdown source file that looks like this:
```md
# Coffeeshop
> Smells nice

* Cost: 20
* Produces: [[coffee.resource]]
```

### StaticDataImporter
Write a `StaticDataImporter` class for your `StaticDataObject`. We're using the `MarkdownStaticDataImporter` as basis:
```csharp
[ScriptedImporter(1, ".building.md")]
public class BuildingDataImporter : MarkdownStaticDataImporter<BuildingData>
{
    // Additional, Unity-specific data that is attached in the editor (it doesn't belong in the source file).
    [SerializeField]
    private Building buildingPrefab;

    protected override BuildingData CreateAndDeserializeInstance(StructuredMarkdownDocument document, string id)
    {
        // Create the main asset instance.
        // Can be called with a generic type argument if the document specifies a more specific type.
        var instance = CreateMainAsset();
        
        // Get the title form the document
        instance.title = document.Title;
        
        // Read the description from the paragraph
        instance.Description = document.GetText(string.Empty);

        // Parse the property list
        var properties = document.GetList(string.Empty).AsDictionary();
        // Parse the integer from the "Cost: ?" line
        instance.Cost = parser.ParseInt(properties["cost"]);
        // Parse a reference to another static data file
        instance.producedGood = parser.ParseReference<ResourceData>(properties["produces"]);

        // Add the building prefab that's defined in the importer
        instance.BuildingPrefab = buildingPrefab;
        
        return instance;
    }
}
```

### StaticDataReference
The `StaticDataReference<T>` type allows you to reference a static data object safely and independently of how you load it.
It is also used by `StaticDataImporter`s for cross-referencing.

A `StaticDataReference<T>` will store the `Id` of a `StaticDataObject`, but not reference the object itself.
At runtime, using `StaticDataReference<T>.Get()` assumes that all valid static data objects are loaded into the `StaticDataLibrary` (see below)
and retrieve the referenced object from it.

In edit mode, `Get()` will use `AssetDatabase` to load the referenced object.
This can be slow, but that makes it safe for loading data that is relevant for editor tools.

**Warning**: Renaming a referenced static data file is currently not properly supported.
The `StaticDataReference` _will_ store the asset's guid, but at runtime, the `Id` will be used.
A mismatch between the `Id` and the guid will be fixed by the `StaticDataReference` PropertyDrawer,
but if none is displayed, nothing will fix the mismatch during a build.

## Setup
The `StaticDataLibrary` allows static data objects to reference each other through `StaticDataReference`s.

During early initialization, it needs to be defined how the `StaticDataLibrary` loads static data objects to accomplish this.

This example uses `Resources.Load` for loading the an object.
```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
private static void Initialize()
{
    StaticDataLibrary.LoadFunction = (type, id) => (StaticDataObject)Resources.Load($"Static Data/{id}", type);
}
```

If the project uses Addressables, it should probably use that instead for loading at runtime.
In that case, it's likely that everything should be loaded ahead of time.
Use `StaticDataLibrary.Register` to register all loaded static data objects.

## Installation

In Unity: **Window > Package Manager > + > Add package from git URL**, then enter:

```
https://github.com/tea-spoons/static-data.git
```

Pin a release by appending a tag, for example `#v0.23.0`.

### Dependencies

None. Static Data works on its own and installs from the git URL without adding anything else.

It uses the optional packages below when your project has them (Unity detects them automatically) and falls back to plain behaviour when it does not.

| Package | Used for |
|---|---|
| Structured Documents (`com.tea-spoons.structured-documents` 0.4.0+) | The importers: `XmlStaticDataImporter`, `JsonStaticDataImporter` (also needs Newtonsoft Json) and the base `StaticDataImporter`. **Without it there are no importers**; the rest of the package (`StaticDataObject`, `StaticDataLibrary`, `StaticDataReference` and the value parsers) works, so you can still create and load static data yourself. |
| Collections (`com.tea-spoons.collections` 0.7.0+) | The two-level dictionary inside `StaticDataLibrary`. Without it the library uses a small built-in equivalent. |
| Package Core (`com.tea-spoons.package-core` 1.3.2+) | A helper of the `StaticDataReference` property drawer. Without it the drawer uses a small built-in equivalent. |

## Notes

Markdown documents (which needed the Markdig library) are not included in this release. JSON support is unchanged.

## Change plan

See [CHANGE-PLAN.md](CHANGE-PLAN.md) for what changed before publishing and what is planned next.

## License

Copyright (c) 2026 Bigpoint. Authored by Muhammad Tarek Abdou.

Available for research, education and other noncommercial use under the [PolyForm Noncommercial 1.0.0](LICENSE.md)
license. Commercial use is not permitted.
