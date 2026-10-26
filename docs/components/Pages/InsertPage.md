# Insert Page

Builds one insert_page Change JSON op per Document JSON item.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Document JSON | Text (list) | Yes | - | Page Document JSON item(s). |
| Into | Text (list) | No | - | Parent page ID, index-matched to Document JSON (blank = space root). |
| At | Integer (list) | No | - | Position among siblings, index-matched to Document JSON. |
| Ref | Text (list) | No | - | Reference key, index-matched to Document JSON. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Change JSON | Text (list) | One insert_page Change JSON op per item. |

## Output

Builds `{"operation": "insert_page", "title", "icon", "document", "parent": "<Into>", "position": <At>, "ref": "<Ref>"}` per item, copying title/icon/document from the Document JSON. Optional fields are omitted when blank. No network call.

## Notes

- Wire a list of Document JSON to batch-create many pages in one Change Request.
- Shorter Into/At/Ref lists leave the remaining items at their defaults (space root, no position, no ref).

## Related

- [Build Page (Markdown)](BuildPageMarkdown.md), [Build Page (JSON)](../Rich%20Content/BuildRichPage.md): Document JSON sources.
- [Content Changes Aggregator](../Compose/ContentChangesAggregator.md)
- [Apply Content Changes](../Change%20Requests/ApplyContentChanges.md)
