# Update Page

Builds one update_page Change JSON op per Page ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Page ID | Text (list) | Yes | - | Page ID(s) to update. |
| Document JSON | Text (list) | No | - | New Document JSON, index-matched to Page ID. |
| Into | Text (list) | No | - | New parent page ID, index-matched to Page ID. |
| At | Integer (list) | No | - | New position among siblings, index-matched to Page ID. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Change JSON | Text (list) | One update_page Change JSON op per item. |

## Output

Builds `{"operation": "update_page", "page": "<Page ID>", "title", "icon", "document", "parent": "<Into>", "position": <At>}` per item. Fields are only included when supplied, so an op with only Into/At moves a page without touching its content. No network call.

## Notes

- Get Page IDs from List Pages or Find Page By Path.

## Related

- [Find Page By Path](../Content/GetPageByPath.md)
- [Build Pages From Tree](BuildPagesFromTree.md): mixes inserts and updates from a tree.
- [Content Changes Aggregator](../Compose/ContentChangesAggregator.md)
