# Merge Change Request

Publishes a Change Request's draft content into the space's live content.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `Merged.` on success, `Failed.` on an error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`POST /spaces/{spaceId}/change-requests/{changeRequestId}/merge`

## Notes

- Button-triggered (button label `Merge`).
- **Irreversible via the API**: merged content is live. Review the draft at the Change Request URL first.
- Run Sync Change Request first if the space changed since the Change Request was opened.

## Related

- [Sync Change Request](SyncChangeRequest.md)
- [Get Change Request](GetChangeRequest.md): get the review URL.
