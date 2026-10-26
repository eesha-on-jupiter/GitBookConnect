# Build Page (JSON)

Builds a page's Document JSON from assembled Blocks JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Title | Text | No | "" | Page title. |
| Blocks JSON | Text (list) | Yes | - | Assembled Blocks JSON (from the Blocks Aggregator). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Document JSON | Text | Page Document JSON. |

## Output

Builds `{"title": "<Title>", "document": {"object": "document", "nodes": [<Blocks JSON>...]}}`. Blank items are skipped; each item must be a JSON object. No network call.

## Notes

- The primary page-authoring path for rich, styled content.
- Shown on the **Rich Content** ribbon panel (its source file lives under `Components/Pages`).
- No Icon input: set an icon with Build Page (Markdown) or edit it in GitBook.
- GitBook's JSON document schema was not validated live during the build; test a page end to end before batch use.

## Related

- [Blocks Aggregator](../Compose/PageBlocksAggregator.md): feeds Blocks JSON.
- [Rich Content tab](index.md): the block builders.
- [Insert Page](../Pages/InsertPage.md)
- [Update Page](../Pages/UpdatePage.md)
