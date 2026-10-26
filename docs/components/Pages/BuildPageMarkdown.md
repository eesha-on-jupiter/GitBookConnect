# Build Page (Markdown)

Builds a page's Document JSON from a plain Markdown body.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Title | Text | No | "" | Page title. |
| Markdown Body | Text | Yes | - | Page content as Markdown. |
| Icon | Text | No | "" | Page icon (emoji or GitBook icon token). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Document JSON | Text | Page Document JSON. |

## Output

Builds `{"title": "<Title>", "icon": "<Icon>", "document": {"markdown": "<Markdown Body>"}}`. Title and icon are omitted when blank. No network call.

## Notes

- The lightweight page path; use Build Page (JSON) for styled text, hints, cards and JSON tables.
- Same Document JSON contract as Build Page (JSON): either output wires into Insert Page or Update Page.

## Related

- [Markdown Table From Grid](MarkdownTableFromGrid.md): builds a Markdown Body table.
- [Insert Page](InsertPage.md)
- [Update Page](UpdatePage.md)
