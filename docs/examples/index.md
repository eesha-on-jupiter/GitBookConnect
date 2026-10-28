# Examples

One page per target workflow, each as a component chain you can rebuild on a canvas.

_Demo files pending; these pages are built from the plugin spec, not from saved canvases, so there is no `.gh` to download yet._

## Before any example

- Load the plugin (see [Install](../index.md#install)) and get a personal access token ([Authentication](../authentication.md)).
- Put the token on one Panel and wire it into every HTTP component's `Token`.
- Find a `Space ID` you can edit: **Workspace > List Organizations**, then **Workspace > List Spaces**.
- Examples 01 to 04 and 06 end in the same write chain: **Change Requests > Create Change Request** (click **Create**), its `Change Request ID` into **Change Requests > Apply Content Changes** (click **Apply**), then open Create Change Request's `URL` to review the draft.

## Examples

| Example | Goal |
|---|---|
| [01 - Batch publish pages](01-batch-publish-pages.md) | One GitBook page per tree branch, all in one Change Request |
| [02 - Table page](02-tabular-table-page.md) | Turn a rows x columns grid into a table on a page |
| [03 - Rich styled page](03-rich-styled-page.md) | Styled text, a heading, a hint, an expandable and cards on one page |
| [04 - Insert image](04-insert-image.md) | An image from a file, a Bitmap, a URL or the Rhino viewport |
| [05 - Change Request lifecycle](05-change-request-lifecycle.md) | Open, write, review and merge a Change Request from the canvas |
| [06 - Sync existing pages](06-sync-existing-pages.md) | Rewrite pages that already exist when the model changes |
