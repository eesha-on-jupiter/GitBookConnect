# 06 - Sync existing pages

_Demo file pending; built from the plugin spec, not a saved canvas._

**Goal:** rewrite pages that already exist when the model changes.

## Components, in wiring order

1. [Find Page By Path](../components/Content/GetPageByPath.md) (Content)
2. [Build Page (Markdown)](../components/Pages/BuildPageMarkdown.md) → [Update Page](../components/Pages/UpdatePage.md) (Pages), or [Build Pages From Tree](../components/Pages/BuildPagesFromTree.md) for many pages
3. [Content Changes Aggregator](../components/Compose/ContentChangesAggregator.md) (Compose)
4. [Create Change Request](../components/Change%20Requests/CreateChangeRequest.md) → [Apply Content Changes](../components/Change%20Requests/ApplyContentChanges.md) (Change Requests)

## Wiring

- Find Page By Path: `Token`, `Space ID`, a slug → `Page Path`. Click **Find**; read the page `id` from `Response`.
- Page IDs → Update Page `Page ID`; regenerated pages from Build Page (Markdown) → `Document JSON` (index-matched).
- Or, for many pages, fill the `Page ID` tree of Build Pages From Tree: filled items become `update_page`, blank items `insert_page`.
- `Change JSON` → Content Changes Aggregator → Apply Content Changes on a new Change Request.

## Inputs you set

- Page slugs, regenerated titles and bodies, `Space ID`, `Token`.

## Result

- The existing pages are rewritten in the Change Request draft; merge to publish.
