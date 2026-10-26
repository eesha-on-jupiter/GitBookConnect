# Content Changes Aggregator

Flattens every wired Change JSON into one ordered batch, capped at the API limit of 50.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Change JSON | Text (list) | Yes | - | Change JSON items, in the order they should be applied. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Change JSON | Text (list) | Validated batch, capped at 50 items. |

## Output

Passes through every item that parses as a JSON object, in order. Invalid items are skipped with a warning; items beyond 50 are dropped with a warning. No network call.

## Notes

- Mixes sources freely: Insert Page, Update Page, Build Pages From Tree and Build Insert File.
- Put Build Insert File before the pages that reference its images.
- More than 50 changes: split them across several Apply Content Changes calls.

## Related

- [Apply Content Changes](../Change%20Requests/ApplyContentChanges.md)
