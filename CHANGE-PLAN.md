# Change plan

> Draft. This file tracks what changed on the way to this repo and what I plan to change next. Edit freely.

## Origin

Originally developed at Bigpoint. Published here with Bigpoint's permission for research, education and other noncommercial use. Copyright (c) 2026 Bigpoint; see [LICENSE.md](LICENSE.md).

## Changes made before publishing

- Namespaces are now `TeaSpoons.*` and the package id is `com.tea-spoons.static-data` (assemblies renamed to match).
- Internal build, registry and tracker references were removed; the repo uses GitHub Actions (`CI` and `Release`) built on `unity-ci-kit`.
- Added `LICENSE.md` (PolyForm Noncommercial 1.0.0), an install section in the README, and package metadata (author, license and documentation URLs).
- Markdown support is disabled: it needed a Markdig integration that is not public. The `MARKDIG` code paths stay compiled out; JSON is unchanged.

## Planned changes

- [x] Tag and publish `v0.22.2` with the Release workflow.
- [ ] Run this package's tests in CI with `unity-ci-kit` (needs a small test-project helper in the kit).
- [ ] Make installs resolve dependencies automatically, for example through a registry such as OpenUPM.
- [ ] Restore Markdown support as an optional integration with the public Markdig library.

## Notes and ideas

_Add your own here._
