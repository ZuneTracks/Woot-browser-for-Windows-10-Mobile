# Changelog

## v1.1.0.2 - About dialog disclaimer

- Added a disclaimer to both About dialogs stating that the app is not affiliated
  with Woot!, Amazon, or any of their affiliates.
- Updated the package version to `1.1.0.2`.
- Published a signed ARM AppX with the matching `WootDevelopment.cer`.

## v1.1.0.1 - Live tile and About updates

- Improved Windows 10 Mobile live-tile notification compatibility with supported
  square and wide text templates plus legacy fallbacks.
- Added diagnostics around live-tile XML generation and notification submission.
- Updated the About dialog in MainPage and SettingsPage to read the installed
  package version dynamically from the app manifest.
- Added the current Woot! app description to the About dialog.
- Published a signed ARM AppX with the matching `WootDevelopment.cer`.

## v1.0.9.0 - Store packaging preparation

- Added an explicit ARM Store-upload configuration that creates an unsigned
  `.appxupload` candidate with public symbols.
- Added a build-time guard requiring Visual Studio Store association metadata
  before Store packaging.
- Updated the package version to use a Store-compatible fourth version field of
  `0`.

## v1.0.7.4 - First public release

- Added a Windows 10 Mobile 15063+ ARM UWP app with a Metro-inspired YourTube UI.
- Added public YouTube Data API v3 search, regional trending, details, channels,
  categories, and inline category expansion.
- Added an official YouTube mobile watch page in-app viewing route with a browser
  fallback and no direct media extraction.
- Added foreground Trending Now live-tile updates.
- Added OAuth 2.0 authorization code plus PKCE architecture with system-browser
  sign-in and Credential Locker storage.
- Removed retained legacy credentials from the recovered source and excluded the
  recovered WP8 projects from the public release repository.

## Security and distribution notes

- No API key, OAuth client ID, OAuth client secret, access token, refresh token, or
  private signing certificate is included.
- The included AppX is for Developer Mode sideloading and is signed with a temporary
  development certificate. It is not a production or Microsoft Store package.
