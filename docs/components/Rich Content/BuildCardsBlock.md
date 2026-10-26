# Cards Block

Builds a cards layout (a columns block) from parallel lists of card titles, texts and icons.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Card Title | Text (list) | Yes | - | Card titles, one card per item. |
| Card Text | Text (list) | No | - | Card body text, index-matched to Card Title. |
| Card Icon | Text (list) | No | - | Card icons, index-matched to Card Title. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Block JSON | Text | Cards (columns) block JSON. |

## Output

Builds `{"object": "block", "type": "columns", "nodes": [...]}` with one `column` per card holding a `heading-3` title, an optional paragraph and an optional `icon`. No network call.

## Notes

- GitBook has no dedicated card block: its Cards look is a columns block, which is what this builds.
- Card text is plain (unstyled).

## Related

- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): collect blocks for Build Page (JSON).
