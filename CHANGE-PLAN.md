# Change plan

> Draft. This file tracks what changed on the way to this repo and what I plan to change next. Edit freely.

## Origin

Originally developed at Bigpoint. Published here with Bigpoint's permission for research, education and other noncommercial use. Copyright (c) 2026 Bigpoint; see [LICENSE.md](LICENSE.md).

## Changes made before publishing

- Namespaces are now `TeaSpoons.*` and the package id is `com.tea-spoons.static-data` (assemblies renamed to match).
- Internal build, registry and tracker references were removed; the repo uses GitHub Actions (`CI` and `Release`) built on `unity-ci-kit`.
- Added `LICENSE.md` (PolyForm Noncommercial 1.0.0), an install section in the README, and package metadata (author, license and documentation URLs).
- Markdown support is disabled: it needed a Markdig integration that is not public. The `MARKDIG` code paths stay compiled out; JSON is unchanged.
- 0.23.0: `com.tea-spoons.collections`, `com.tea-spoons.structured-documents` and `com.tea-spoons.package-core` are no longer dependencies. Each is used when the project has it (`TEASPOONS_COLLECTIONS`, `TEASPOONS_STRUCTURED_DOCUMENTS`, `TEASPOONS_PACKAGE_CORE`, set from the asmdef `versionDefines`). Without collections and package-core, small built-in stand-ins are used (`DictionaryDictionary`, `SerializedPropertyExtensions`). Without structured-documents the importers are compiled out. Added tests for the stand-in that run without package-core.

## Planned changes

- [x] Tag and publish `v0.22.2` with the Release workflow.
- [x] Tag and publish `v0.23.0` with the Release workflow.
- [ ] Run this package's tests in CI with `unity-ci-kit` (needs a small test-project helper in the kit).
- [ ] Restore Markdown support as an optional integration with the public Markdig library.
<!-- review-items:start -->
- [ ] **P1** Validate references at import time: after an import, check that every `StaticDataReference` id resolves and report the asset and field in the Console (BakingSheet validates typed cross-sheet references).
- [ ] **P1** Add a CSV importer (tabular data) as another `StaticDataImporter`, and document the Google Sheets export workflow.
- [ ] **P1** Declares `unity: 2022.3`, but only Unity 6000.3.8f1 was tested. Add a Unity version matrix to CI once package tests run there (see the `unity-ci-kit` plan), or raise the minimum.
- [ ] **P2** Optionally bake all objects into one file at build time and load that at runtime (BakingSheet converts to JSON at edit time).
- [ ] **P2** Provide a test helper that resets `StaticDataLibrary` and `LoadFunction`.
- [ ] **P2** Add a `CHANGELOG.md`. Unity's package layout lists one next to `README.md`, and the `unity-ci-kit` validator warns without it.
<!-- review-items:end -->

<!-- review:start -->
## Review (September 2026)

Reviewed as a senior Unity engineer would: I read the code and compared the package with similar open-source projects (September 2026). Those projects are listed for ideas only. Nothing was copied from them, and their licenses are noted in case code is ever reused. Priorities: **P0** correctness bug or broken metadata, **P1** should be done soon, **P2** nice to have.

### Compared with

| Project | License | Worth noting |
|---|---|---|
| [cathei/BakingSheet](https://github.com/cathei/BakingSheet) | MIT | Schema as C# classes; imports Excel, Google Sheets, CSV and JSON. Typed references between sheets are validated. Data is converted to JSON at edit time, so the runtime does not need the heavy converters. `AssetPostProcessor` automation, a ScriptableObject converter and `link.xml` for AOT. |
| [tlauterbach/potato-sheets](https://github.com/tlauterbach/potato-sheets) | not checked | Google Sheets importer for ScriptableObjects. |

### Findings from reading the code

- **[Gap]** Importers exist for JSON and XML; Markdown is compiled out. There is no CSV, Excel or Google Sheets path, which is how designers usually author this kind of data.
- **[Validation]** A `StaticDataReference` stores an id and resolves at runtime. `StaticDataLibrary.Get<T>` throws `KeyNotFoundException` when the id is missing, so a wrong reference shows up in play, not at import.
- **[State]** `StaticDataLibrary` and its `LoadFunction` are static, so tests can leak state into each other.
<!-- review:end -->

## Notes and ideas

_Add your own here._
