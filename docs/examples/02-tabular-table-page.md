# 02 - Table page

_Demo file pending — built from the plugin spec, not a saved canvas._

**Goal:** turn a rows x columns grid (a schedule, a takeoff, a load-case summary) into a table on a GitBook page.

## Components, in wiring order

JSON-block path:

1. [Table Block](../components/Rich%20Content/TableBlock.md) (Rich Content)
2. [Blocks Aggregator](../components/Compose/PageBlocksAggregator.md) (Compose)
3. [Build Page (JSON)](../components/Rich%20Content/BuildRichPage.md) (Rich Content)
4. [Insert Page](../components/Pages/InsertPage.md) (Pages)
5. [Content Changes Aggregator](../components/Compose/ContentChangesAggregator.md) (Compose)
6. [Apply Content Changes](../components/Change%20Requests/ApplyContentChanges.md) (Change Requests)

Markdown path: [Markdown Table From Grid](../components/Pages/MarkdownTableFromGrid.md) → [Build Page (Markdown)](../components/Pages/BuildPageMarkdown.md) → Insert Page, then steps 5-6.

## Wiring

- A grid tree (one branch per row, one item per column) → Table Block `Grid`; a list of column names → `Header Row`.
- `Block JSON` → Blocks Aggregator `Block JSON` → its `Blocks JSON` → Build Page (JSON) `Blocks JSON`; a Panel → `Title`.
- `Document JSON` → Insert Page `Document JSON` → `Change JSON` → Content Changes Aggregator → Apply Content Changes (with a Change Request ID from Create Change Request).
- Markdown variant: the same grid → Markdown Table From Grid `Grid` / `Header Row`, its `Markdown Body` → Build Page (Markdown) `Markdown Body`, then `Document JSON` → Insert Page as above.

## Inputs you set

- The grid tree, the header list, the page title, `Space ID`, `Token`.

## Result

- A new page holding the table, in the Change Request draft. The Markdown path is the one validated live; test the JSON path on one page before batch use.
