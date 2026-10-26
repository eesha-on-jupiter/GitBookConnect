# Get Published Content

Retrieves a space's currently published (merged) content.

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

`GET /spaces/{spaceId}/content`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Read-only: shows live content only, never unmerged Change Request drafts.

## Related

- [List Pages](GetAllPages.md)
- [Get Change Request Content](../Change%20Requests/GetChangeRequestContent.md): the draft-side equivalent.
