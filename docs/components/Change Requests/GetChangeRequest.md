# Get Change Request

Retrieves one Change Request by ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |

## Outputs

| Name | Type | Description |
|---|---|---|
| URL | Text | Direct link to view or edit the Change Request in the GitBook web app (`urls.app`). |
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /spaces/{spaceId}/change-requests/{changeRequestId}`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Open URL in a browser to review applied changes before merging.

## Related

- [Get Change Request Content](GetChangeRequestContent.md)
- [Create Change Request](CreateChangeRequest.md)
