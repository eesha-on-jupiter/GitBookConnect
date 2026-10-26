# Paragraph Block

Wraps one or more styled Text JSON runs into a paragraph block.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Text JSON | Text (list) | Yes | - | Styled Text JSON runs, in order (from the Text tab). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Block JSON | Text | Paragraph block JSON. |

## Output

Builds `{"object": "block", "type": "paragraph", "nodes": [{"object": "text", "leaves": [<runs>]}]}`. No network call.

## Notes

- Several runs in one list make one paragraph with mixed styling, e.g. plain, then bold, then plain.

## Related

- [Text tab](../Text/index.md): build runs.
- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): collect blocks for Build Page (JSON).
