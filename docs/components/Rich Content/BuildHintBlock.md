# Hint Block

Builds a hint (callout) block from styled Text JSON runs.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Style | Text | No | "info" | Hint style (info / warning / danger / success). |
| Text JSON | Text (list) | Yes | - | Styled Text JSON runs, in order (from the Text tab). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Block JSON | Text | Hint block JSON. |

## Output

Builds `{"object": "block", "type": "hint", "data": {"style": "<Style>"}, "nodes": [<paragraph>]}`. A blank Style falls back to `info`. No network call.

## Related

- [Hint Style](../Presets/HintStylePreset.md) preset.
- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): collect blocks for Build Page (JSON).
