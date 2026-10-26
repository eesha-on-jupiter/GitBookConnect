# Sync Change Request

Rebases a Change Request on the space's latest live content before merging.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `Synced.` after a rebase, `Already up-to-date.` when there was nothing to rebase, `Failed.` on an error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`POST /spaces/{spaceId}/change-requests/{changeRequestId}/update`

## Notes

- Button-triggered (button label `Sync`).
- GitBook answers HTTP 409 "Branch is already up-to-date" when nothing changed upstream; this is reported as `Already up-to-date.`, not as an error.

## Related

- [Merge Change Request](MergeChangeRequest.md)
