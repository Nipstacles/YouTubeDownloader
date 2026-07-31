# Changelog

## 1.0.4 - 2026-07-31

### Added
- Download queue with sequential processing for multiple YouTube URLs.
- Persistent download history with timestamp, URL, quality, output folder, status, and failure details.
- Audio metadata options for audio-only downloads.
- Editable audio tags: title, artist, album, genre, year, and track.
- Optional auto-fill of editable audio tags from YouTube metadata.
- Optional thumbnail cover art embedding for compatible audio formats.

### Improved
- Metadata preview now handles null JSON fields from yt-dlp and shows useful failure details.
- Metadata preview reads yt-dlp output more safely to avoid pipe blocking.
- Status checks now avoid overlapping YouTube plan checks and reduce large temporary allocations.
- MP3 metadata compatibility improved with ID3v2.3 tagging.
- Download history is capped to the most recent 100 items.

### Fixed
- Repeated Status clicks could create excessive Large Object Heap pressure.
- Metadata preview could fail when yt-dlp returned null duration or format height values.
- Queue downloads now continue after non-cancel failures and record failures in history.
