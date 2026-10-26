# Plain Text

Wraps a raw string into a Text JSON leaf with no marks.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Text | Text | No | "" | Raw text. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Text JSON | Text | Text JSON leaf, no marks. |

## Output

Builds `{"object": "leaf", "text": "<Text>", "marks": []}`. No network call.

## Notes

- The starting point of every styled run: chain Mark components after it, or use it unstyled.

## Related

- [Bold Mark](BoldMark.md) and the other marks.
- [Paragraph Block](../Rich%20Content/ParagraphBlock.md): takes Text JSON runs.
