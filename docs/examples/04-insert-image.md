# 04 - Insert image

_Demo file pending; built from the plugin spec, not a saved canvas._

**Goal:** put an image on a page from a file, a Bitmap, a URL or a live Rhino viewport capture.

## Components, in wiring order

1. [Build Insert File](../components/Compose/BuildInsertFile.md) (Compose)
2. [Blocks Aggregator](../components/Compose/PageBlocksAggregator.md) (Compose)
3. [Build Page (JSON)](../components/Rich%20Content/BuildRichPage.md) (Rich Content)
4. [Insert Page](../components/Pages/InsertPage.md) (Pages)
5. [Content Changes Aggregator](../components/Compose/ContentChangesAggregator.md) (Compose)
6. [Apply Content Changes](../components/Change%20Requests/ApplyContentChanges.md) (Change Requests)

## Wiring

- Build Insert File: one item per image in `File Name` (e.g. `plan.png`, `view.png`).
- Index-matched sources: a path → `File Path` for item 0, `True` → `Capture Rhino Viewport` for item 1 (optional `View Name`, e.g. `Perspective`); a Bitmap → `Bitmap` or a link → `Source URL` work the same way.
- Optional keys → `Ref` (one per image; blank or missing items get an auto-generated key). Click **Build**.
- `Block JSON` → Blocks Aggregator `Block JSON` (with any other blocks) → Build Page (JSON) → Insert Page.
- Build Insert File `Change JSON` first, then Insert Page `Change JSON` → Content Changes Aggregator `Change JSON` → Apply Content Changes.

## Inputs you set

- File names, one source per item, optional view names and refs.

## Result

- The files and the page that shows them land in the same Change Request draft. Base64 content is capped at 1MB by the API.
