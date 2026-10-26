# Connect to GitBook

Validates a GitBook Personal Access Token and returns the connected account.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | GitBook Personal Access Token (Bearer). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `Connected.` on success, `Failed to authenticate.` on an error, `No token supplied.` when Token is empty. |
| Response | Text | Raw `GET /user` response JSON (your user account), or the error text on failure. |

## API endpoint

`GET /user`

## Notes

- Button-triggered (button label `Connect`): the request only runs on a click, never on canvas recompute.
- Use a **Personal Access Token**, not an integration token: GitBook rejects integration tokens on Change Request writes.
- A valid token still carries your space role: Reviewer and Viewer roles cannot create or apply Change Requests.
- Run this first on a new canvas: if it fails, nothing downstream will work.

## Related

- [List Organizations](../Workspace/GetAllOrganizations.md): next step, find your organization ID.
- [Authentication](../../authentication.md): how to create the token.
