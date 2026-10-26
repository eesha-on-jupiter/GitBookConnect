# Get Space

Retrieves one GitBook space by ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /spaces/{spaceId}`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.

## Related

- [List Spaces](GetAllSpaces.md)
- [Get Published Content](../Content/GetPublishedContent.md)
