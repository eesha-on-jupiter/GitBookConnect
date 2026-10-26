# Find Page By Path

Retrieves one published page by its path (slug) rather than its ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Page Path | Text | Yes | - | Page path (slug), e.g. `getting-started/quickstart`. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /spaces/{spaceId}/content/path/{pagePath}`

## Notes

- Button-triggered (button label `Find`): the request only runs on a click, never on canvas recompute.
- The path is URL-encoded before sending.
- Useful for re-syncing: find a page by a stable slug, read its `id`, then wire it into Update Page.

## Related

- [Get Page](GetPage.md)
- [Update Page](../Pages/UpdatePage.md)
