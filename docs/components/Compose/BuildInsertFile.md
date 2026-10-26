# Build Insert File

Builds an insert_files Change JSON and matching Image Blocks from a file path, Bitmap, URL, base64 string or Rhino viewport capture.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| File Name | Text (list) | Yes | - | File name(s), e.g. `diagram.png`; one item per file. |
| File Path | Text (list) | No | - | Local file path(s) to read from disk. |
| Bitmap | Generic (list) | No | - | GH Bitmap object(s), PNG-encoded. |
| Source URL | Text (list) | No | - | Externally hosted URL(s) for GitBook to fetch. |
| Base64 | Text (list) | No | - | Inline base64-encoded file content (capped at 1MB by the API). |
| Capture Rhino Viewport | Boolean (list) | No | - | Capture the active Rhino 3D view for this item (takes priority when true). |
| View Name | Text (list) | No | - | Named Rhino viewport to capture (blank = the currently active viewport). |
| Ref | Text (list) | No | - | Reference key linking each file to its Image Block (blank = auto-generated). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Change JSON | Text | One insert_files Change JSON op covering every item. |
| Block JSON | Text (list) | One Image Block per item. |

## Output

Builds `{"operation": "insert_files", "files": [{"name", "ref", "content" | "url"}...]}` plus one `{"object": "block", "type": "images", "nodes": [{"type": "image", "data": {"ref": "<Ref>"}}]}` per file. No network call.

## Notes

- Button-triggered (button label `Build`): outputs refresh on a click, so a viewport capture is taken at that moment.
- Per item, the first source set wins: Capture Rhino Viewport, then File Path, Bitmap, Source URL, Base64. Items with no source are skipped with a warning.
- All inputs are index-matched to File Name.
- Ref is optional: a missing or blank item gets an auto-generated key.
- Send Change JSON through Content Changes Aggregator (before the page ops) and the Block JSON through Blocks Aggregator into the page.

## Related

- [Content Changes Aggregator](ContentChangesAggregator.md)
- [Blocks Aggregator](PageBlocksAggregator.md)
