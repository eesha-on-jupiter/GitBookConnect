# Update Change Request

Updates an existing Change Request's subject and/or description.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |
| Subject | Text | No | "" | New subject. |
| Description | Text | No | "" | New description (sent as Markdown). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an error, `Nothing to update.` when both Subject and Description are blank. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`PATCH /spaces/{spaceId}/change-requests/{changeRequestId}`

## Notes

- Button-triggered (button label `Update`).
- Only the fields supplied are sent; a blank field is left unchanged.

## Related

- [Create Change Request](CreateChangeRequest.md)
