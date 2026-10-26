# Table Block

Builds a GitBook table block from a Header Row and a rows x columns Grid tree.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Header Row | Text (list) | Yes | - | Column headers. |
| Grid | Text (tree) | Yes | - | Cell data: one branch per row, one item per column. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Block JSON | Text | Table block JSON. |

## Output

Builds `{"object": "block", "type": "table", "data": {"view": "grid"}, "nodes": [<table-row>...]}` with a header row flagged `"header": true` and plain-text `table-cell`s. No network call.

## Notes

- v1 scope: plain text cells in a grid view; typed columns and card-view tables are not covered.
- Header Row is not flagged optional, so it must be wired.

## Related

- [Markdown Table From Grid](../Pages/MarkdownTableFromGrid.md): the Markdown equivalent.
- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): collect blocks for Build Page (JSON).
