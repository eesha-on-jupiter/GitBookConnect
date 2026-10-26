# Markdown Table From Grid

Builds a Markdown table from a Header Row and a rows x columns Grid tree.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Header Row | Text (list) | No | - | Column headers. |
| Grid | Text (tree) | Yes | - | Cell data: one branch per row, one item per column. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Markdown Body | Text | Markdown table. |

## Output

Builds `| h1 | h2 |`, a `| --- |` separator row, then one `| a | b |` line per branch. Pipes in cells are escaped and line breaks become spaces. No network call.

## Notes

- The Markdown path for schedules, takeoffs and load-case summaries; Table Block is the JSON equivalent.

## Related

- [Build Page (Markdown)](BuildPageMarkdown.md): takes Markdown Body.
- [Table Block](../Rich%20Content/TableBlock.md)
