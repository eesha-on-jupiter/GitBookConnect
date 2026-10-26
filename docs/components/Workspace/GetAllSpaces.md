# List Spaces

Lists every space in a GitBook organization.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token, sent as a Bearer header. |
| Organization ID | Text | Yes | - | ID of the GitBook organization (from List Organizations). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` on success, `Failed.` on an HTTP or network error. |
| Response | Text | Raw response JSON on success, or the error text (HTTP code, reason and body) on failure. |

## API endpoint

`GET /orgs/{organizationId}/spaces`

## Notes

- Button-triggered (button label `Get`): the request only runs on a click, never on canvas recompute.
- Read a space `id` from Response: every Content and Change Requests component takes it as `Space ID`.

## Related

- [Get Space](GetSpace.md)
- [Create Change Request](../Change%20Requests/CreateChangeRequest.md)
