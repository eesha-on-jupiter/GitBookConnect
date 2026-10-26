# Expandable Block

Builds a collapsible (expandable) block from a title and styled Text JSON runs.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Title | Text | Yes | - | Expandable title. |
| Text JSON | Text (list) | Yes | - | Styled Text JSON runs, in order (from the Text tab). |
| Default Expanded | Boolean | No | false | Whether the block starts expanded. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Block JSON | Text | Expandable block JSON. |

## Output

Builds `{"object": "block", "type": "expandable", "data": {"title": "<Title>", "expanded": <Default Expanded>}, "nodes": [<paragraph>]}`. No network call.

## Related

- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): collect blocks for Build Page (JSON).
