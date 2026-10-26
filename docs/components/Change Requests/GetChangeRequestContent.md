# Get Change Request Content

Retrieves a Change Request's draft content.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /spaces/{spaceId}/change-requests/{changeRequestId}/content`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Shows the draft as it stands after Apply Content Changes, before any merge.

## Related

- [Apply Content Changes](ApplyContentChanges.md)
- [Get Published Content](../Content/GetPublishedContent.md): the live-side equivalent.
