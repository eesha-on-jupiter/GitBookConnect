# Get Page

Retrieves one published page by ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Page ID | Text | Yes | - | ID of the page (from List Pages). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /spaces/{spaceId}/content/page/{pageId}`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.

## Related

- [Find Page By Path](GetPageByPath.md): look a page up by slug instead.
