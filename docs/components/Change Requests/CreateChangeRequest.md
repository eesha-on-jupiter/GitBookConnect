# Create Change Request

Opens a new Change Request (the write branch) on a space.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Subject | Text | No | "" | Change Request subject (title). |
| Description | Text | No | "" | Change Request description, set with a follow-up update call. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Change Request ID | Text | ID of the new Change Request; wire it into every other Change Requests component. |
| URL | Text | Direct link to view or edit the Change Request in the GitBook web app (`urls.app`). |
| Status | Text | `Created.` on success, `Failed.` on an error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`POST /spaces/{spaceId}/change-requests`, then `PATCH /spaces/{spaceId}/change-requests/{changeRequestId}` when Description is set

## Notes

- Button-triggered (button label `Create`): each click opens a new Change Request.
- GitBook only accepts a subject at creation; a Description is sent with a second `PATCH` call. If that call fails, the Change Request still exists and a warning is shown.
- Requires a Personal Access Token and an Editor (or higher) role on the space.

## Related

- [Apply Content Changes](ApplyContentChanges.md): write content into it.
- [Update Change Request](UpdateChangeRequest.md)
- [Merge Change Request](MergeChangeRequest.md)
