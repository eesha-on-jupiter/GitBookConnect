# 01 - Batch publish pages

_Demo file pending — built from the plugin spec, not a saved canvas._

**Goal:** one GitBook page per tree branch (one level, zone or element type each), all in one Change Request.

## Components, in wiring order

1. [Build Pages From Tree](../components/Pages/BuildPagesFromTree.md) (Pages)
2. [Content Changes Aggregator](../components/Compose/ContentChangesAggregator.md) (Compose)
3. [Create Change Request](../components/Change%20Requests/CreateChangeRequest.md) (Change Requests)
4. [Apply Content Changes](../components/Change%20Requests/ApplyContentChanges.md) (Change Requests)

## Wiring

- A tree of titles (one branch per level, zone or element type) → Build Pages From Tree `Title`; a matching tree of Markdown → `Body Text`; an optional emoji tree → `Icon`.
- Leave `Page ID` empty so every item becomes an `insert_page`; set `Into` to a parent page ID, or leave it blank for the space root.
- `Change JSON` → Content Changes Aggregator `Change JSON`.
- Create Change Request: token → `Token`, space → `Space ID`, a Panel → `Subject`. Click **Create**.
- Aggregator `Change JSON` → Apply Content Changes `Change JSON`; Create Change Request's `Change Request ID` → `Change Request ID`; token and `Space ID` in. Click **Apply**.

## Inputs you set

- `Title` / `Body Text` / `Icon` trees, `Into`, the Change Request `Subject`, `Space ID`, `Token`.

## Result

- One new page per branch item in the Change Request draft; open its `URL` to review, then [merge](05-change-request-lifecycle.md) when ready.
- More than 50 pages: the aggregator caps a batch at 50, so split the tree across several Apply calls.
