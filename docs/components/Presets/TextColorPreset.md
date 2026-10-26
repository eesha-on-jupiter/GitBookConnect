# Text Color

A value-list preset of GitBook colour tokens, for Color Mark's Text Color and Background Color inputs.

## Inputs

None.

## Outputs

One unnamed value-list output carrying the selected value as a quoted string.

## Values

| Display Name | Value |
|---|---|
| Default | `default` |
| Green | `green` |
| Blue | `blue` |
| Red | `red` |
| Orange | `orange` |
| Yellow | `yellow` |
| Purple | `purple` |
| Pink | `pink` |
| Violet | `violet` |
| Cyan | `cyan` |
| Primary (theme) | `$primary` |
| Info (theme) | `$info` |
| Success (theme) | `$success` |
| Warning (theme) | `$warning` |
| Danger (theme) | `$danger` |
| Tint (theme) | `$tint` |

## Feeds into

- [Color Mark](../Text/ColorMark.md)'s `Text Color` and `Background Color` inputs.

## Notes

- `$` tokens resolve from the published site's theme; named colours are fixed.
- `default` means no colour and is left out of the mark.

## Related

- [Color Mark](../Text/ColorMark.md)
