# Subscript Mark

Adds a subscript mark to a Text JSON leaf.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Text JSON | Text | Yes | - | Text JSON leaf. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Text JSON | Text | Text JSON leaf, with subscript added. |

## Output

Appends `{"type": "subscript"}` to the leaf's `marks` array. Re-applying the same mark replaces it rather than duplicating it. No network call.

## Notes

- Chain onto another mark's output to combine styles, e.g. Plain Text > Bold Mark > Italic Mark.

## Related

- [Plain Text](PlainText.md): start a run.
- [Rich Content tab](../Rich%20Content/index.md): wrap runs into blocks.
