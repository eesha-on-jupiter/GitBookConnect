# Request Reviewers

Requests one or more reviewers for a Change Request, by GitBook user ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Space ID | Text | Yes | - | ID of the GitBook space. |
| Change Request ID | Text | Yes | - | ID of the Change Request (from Create Change Request or List Change Requests). |
| Reviewer User ID | Text (list) | Yes | - | GitBook user ID(s) to request as reviewers. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`POST /spaces/{spaceId}/change-requests/{changeRequestId}/requested-reviewers`

## Notes

- Button-triggered (button label `Request`).
- Blank IDs are skipped; an all-blank list fails with `Reviewer User ID list is empty.`
- Requires a Personal Access Token.

## Related

- [Create Change Request](CreateChangeRequest.md)
