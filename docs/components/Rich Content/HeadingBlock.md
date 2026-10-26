# Heading Block

Wraps one or more styled Text JSON runs into a heading block.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Text JSON | Text (list) | Yes | - | Styled Text JSON runs, in order (from the Text tab). |
| Level | Integer | No | 2 | Heading level (1-6). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Block JSON | Text | Heading block JSON. |

## Output

Builds `{"object": "block", "type": "heading-<Level>", "nodes": [...]}`. Level is clamped to 1-6. No network call.

## Related

- [Paragraph Block](ParagraphBlock.md)
- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): collect blocks for Build Page (JSON).
