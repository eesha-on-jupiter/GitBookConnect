# Blocks Aggregator

Gathers Block JSON from Rich Content blocks and Build Insert File into the Blocks JSON list for Build Page (JSON).

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Block JSON | Text (list) | Yes | - | Block JSON items, in the order they should appear on the page. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Blocks JSON | Text (list) | Validated Block JSON items, in order. |

## Output

Passes through every item that parses as a JSON object; invalid items are skipped with a warning. No network call.

## Notes

- Wire several block outputs into the one input (hold Shift) in reading order.

## Related

- [Build Page (JSON)](../Rich%20Content/BuildRichPage.md)
- [Rich Content tab](../Rich%20Content/index.md)
