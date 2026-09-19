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

## Notes and ideas

_Add your own here._
