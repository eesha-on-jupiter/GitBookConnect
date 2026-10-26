# Build Pages From Tree

Builds one insert_page or update_page Change JSON op per branch item from parallel trees of page properties.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Into | Text | No | "" | Shared parent page ID for every new page (blank = space root). |
| Page ID | Text (tree) | No | - | Existing page ID per branch item (blank = insert a new page instead of updating). |
| Title | Text (tree) | No | - | Page title per branch item. |
| Body Text | Text (tree) | No | - | Page body (Markdown) per branch item. |
| Icon | Text (tree) | No | - | Page icon per branch item. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Change JSON | Text (list) | One insert_page/update_page Change JSON op per branch item. |

## Output

For every branch path (union across the four trees, sorted) and every item index, builds a Markdown Document JSON from Title/Body Text/Icon, then emits `insert_page` when Page ID is blank or `update_page` when it is set. No network call.

## Notes

- The parametric bulk path: one branch per level, zone, room or element type gives one page each.
- Branches are matched by path; within a branch, items are matched by index and the longest list wins.
- Body Text is Markdown; for styled JSON pages use Build Page (JSON) with Insert Page instead.

## Related

- [Insert Page](InsertPage.md)
- [Update Page](UpdatePage.md)
- [Content Changes Aggregator](../Compose/ContentChangesAggregator.md)
