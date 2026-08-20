# Microsoft Store readiness

Assessment of the authoritative `YouTube.Uwp.sln` source at the v1.0.8.4
development-package state, prepared as package version `1.0.9.0`.

## Result

The app is a classic C# UWP project that can technically produce an ARM
`.appx`/`.appxbundle` and an `.appxupload` candidate with the installed Visual
Studio/MSBuild and Windows 10 SDK tooling. It is **not upload-ready**: the
Store profile now stops before packaging because the required Partner Center
identity is intentionally absent.

Windows 10 Mobile ARM remains a technically valid UWP architecture/device-family
combination in the package model, but Windows 10 Mobile reached end of support
on December 10, 2019. Whether a new listing or an update can be offered to that
device family is a Partner Center/Store decision; this repository cannot
establish that eligibility.

## Current state inspected

- Source solution: `YouTube.Uwp.sln`; the nested `YourTube-UWP` tree is retained
  recovered material and is not referenced by this solution.
- Target device family: `Windows.Mobile`.
- Minimum and tested OS: `10.0.15063.0`.
- Architecture: ARM only.
- Existing v1.0.8.4 output: `Debug` ARM sideload package, signed with the
  `YourTubeDevelopment` certificate and containing debug framework dependencies.
- Existing package identity: `Name="YourTube"`,
  `Publisher="CN=YourTubeDevelopment"`, and
  `PublisherDisplayName="YourTube Development"`.
- The existing development PFX and generated packages are ignored by the new
  root `.gitignore` and are not Store deliverables.

## Changes made

- Added a `Store | ARM` solution configuration.
- Configured that profile with `UapAppxPackageBuildMode=StoreUpload`,
  `AppxBundle=Always`, ARM bundle selection, optimized output, and
  `AppxPackageSigningEnabled=false`. Store packages are re-signed by Microsoft;
  no private signing key belongs in the source tree.
- Added a build-time error when `Package.StoreAssociation.xml` is missing.
  This prevents an apparently successful upload candidate from being created
  with the development identity.
- Changed the package version to `1.0.9.0`. Microsoft reserves the fourth
  version component for Store use, so it must be `0`.
- Added ignore rules for generated packages, symbols, certificates, and local
  Store association metadata.

## Exact blockers and user/Partner Center actions

1. Create or access the Microsoft Store developer account and reserve the
   **YourTube** app name in Partner Center.
2. In Visual Studio, use **Publish > Associate App with the Store** for the
   authoritative UWP project while signed in with that Partner Center account.
   This supplies the exact Store-assigned `Identity Name`, `Publisher`,
   `PublisherDisplayName`, Store/PFN metadata, and any required phone identity.
   Do not invent or manually substitute these values.
3. Review the associated manifest and confirm that its package version remains
   `1.0.9.0`, with the fourth component set to `0`.
4. Confirm with Partner Center whether the `Windows.Mobile` target is eligible
   for the intended new listing or update. If PC/ARM64 distribution is desired,
   this project needs a separate supported-device-family and architecture
   scope; ARM Windows 10 Mobile is not ARM64.
5. Complete the Store listing, screenshots, age rating, privacy policy URL if
   personal information is collected/transmitted, pricing/availability, and
   certification notes in Partner Center.
6. Test the optimized package on every intended device family. WACK is
   deprecated and optional; Partner Center performs official certification.

The repository does not contain and must not receive a Store publisher
certificate, private signing key, reserved app name, package family name, Store
ID, Partner Center credentials, or OAuth/API secrets.

## Package generation

After Store association metadata is present, run from the source root in a
Visual Studio developer PowerShell:

```powershell
msbuild .\YouTube.Uwp.sln /t:Restore,Build /p:Configuration=Store /p:Platform=ARM
```

The `Store | ARM` profile requests an optimized ARM bundle and
`UapAppxPackageBuildMode=StoreUpload`, so the expected output is an
`.appxupload` containing the ARM package/bundle and public symbols. The current
machine has MSBuild and the 10.0.15063.0 SDK installed, but no upload artifact
can be considered valid until the Store association values are supplied and
the resulting package is accepted by Partner Center.

Microsoft recommends `.appxupload`/`.msixupload` for Store submission rather
than uploading a raw `.appx`, `.appxbundle`, `.msix`, or `.msixbundle`. An ARM
only upload is technically packageable for this Mobile-only project; it does
not provide x86, x64, or ARM64 coverage.

## Validation performed

- `Store | ARM` restore/build: **fails intentionally** at
  `YouTube.Uwp\YouTube.Uwp.csproj(135,5)` with
  `Store packaging requires Package.StoreAssociation.xml`. This prevents
  packaging with the development identity.
- `Release | ARM` restore/build: **passes**, producing an unsigned
  `1.0.9.0` ARM AppX and public symbols.
- Windows 10 SDK `MakeAppx` unpack of that ARM AppX: **passes**.
- `SignTool verify` on the new Release AppX: reports **No signature found**,
  which is expected for a Store submission candidate. The prior v1.0.8.4
  package instead verifies against the untrusted `YourTubeDevelopment`
  certificate and is not a Store package.
- The installed 10.0.15063.0 `MakeAppx` does not provide a `validate`
  command; Windows App Certification Kit is deprecated and was not used.

## References

- [Packaging UWP apps with Visual Studio](https://learn.microsoft.com/en-us/windows/msix/package/packaging-uwp-apps)
- [View app identity details](https://learn.microsoft.com/en-us/windows/apps/publish/view-app-identity-details)
- [Upload MSIX/AppX packages](https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msix/upload-app-packages)
- [App package requirements](https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msix/app-package-requirements)
- [Windows 10 Mobile end of support](https://learn.microsoft.com/en-us/lifecycle/announcements/windows-10-mobile-end-of-support)
