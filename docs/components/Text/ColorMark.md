# Color Mark

Adds a text and/or background colour mark to a Text JSON leaf.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Text JSON | Text | Yes | - | Text JSON leaf. |
| Text Color | Text | No | "default" | GitBook color token. |
| Background Color | Text | No | "default" | GitBook color token (highlight). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Text JSON | Text | Text JSON leaf, with color added. |

## Output

Appends `{"type": "color", "data": {"text": "<Text Color>", "background": "<Background Color>"}}` to the leaf's `marks`; a `default` colour is left out of `data`. Re-applying replaces the previous color mark. No network call.

## Notes

- Background Color is GitBook's equivalent of a highlight.
- Use the Text Color preset for valid tokens: named colours (`green`, `blue` ...) or theme tokens (`$primary`, `$info` ...).

## Related

- [Text Color](../Presets/TextColorPreset.md) preset.
- [Plain Text](PlainText.md)
